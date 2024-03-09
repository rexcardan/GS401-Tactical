using ESAPIX.Common;
using ESAPIX.Logging;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Helpers
{
    public class Multithread : IDisposable
    {
        private BlockingCollection<Task> _vmsJobs = new BlockingCollection<Task>();
        private BlockingCollection<Task> _nonVMSJobs = new BlockingCollection<Task>();
        private static Multithread instance = null;
        private static readonly object padlock = new object();
        private Thread vmsThread;
        private Thread nonVMSThread;
        private MTContext _sac;
        private CancellationTokenSource cts;

        private static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        private Multithread()
        {
            cts = new CancellationTokenSource();
            nonVMSThread = new Thread(NonVMSThreadStart)
            {
                Name = "ACT NonVMS",
                IsBackground = true
            };
            nonVMSThread.SetApartmentState(ApartmentState.STA);
            nonVMSThread.Start();

            vmsThread = new Thread(VMSThreadStart)
            {
                Name = "ACT VMS",
                IsBackground = true
            };
            vmsThread.SetApartmentState(ApartmentState.STA);
            vmsThread.Start();
        }

        private void NonVMSThreadStart()
        {
            foreach (var job in _nonVMSJobs.GetConsumingEnumerable(cts.Token))
            {
                try
                {
                    job.RunSynchronously();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    OnExceptionRaisedHandler(ex);
                }
            }
        }

        private void VMSThreadStart()
        {
            foreach (var job in _vmsJobs.GetConsumingEnumerable(cts.Token))
            {
                try
                {
                    job.RunSynchronously();
                }
                catch (Exception ex)
                {
                    NonVMSInvoke(() =>
                    {
                        _logger.Error(ex);
                        OnExceptionRaisedHandler(ex);
                    });
                }
            }
        }

        public static Multithread Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Multithread();
                    }
                    return instance;
                }
            }
        }

        public Task SetContext(Func<VMS.TPS.Common.Model.API.Application> createAppFunc)
        {
            return InvokeAsync(new Action(() =>
            {
                Application app = null;
                try
                {
                    app = createAppFunc();
                }
                catch (Exception e)
                {
                    throw e;
                }
                _sac = new MTContext(app);
                _sac.Thread = this;

                NonVMSInvoke(() =>
                {
                    RaiseContextLoaded();
                });
            }));
        }

        public T GetValue<T>(Func<MTContext, T> sacFunc)
        {
            T toReturn = default(T);
            Invoke(() =>
            {
                toReturn = sacFunc(_sac);
            });
            return toReturn;
        }

        public async Task<T> GetValueAsync<T>(Func<MTContext, T> sacFunc)
        {
            T toReturn = default(T);
            await InvokeAsync(() =>
            {
                toReturn = sacFunc(_sac);
            });
            return toReturn;
        }

        public void Execute(Action<MTContext> sacOp)
        {
            Invoke(() =>
            {
                sacOp(_sac);
            });
        }

        public Task ExecuteAsync(Action<MTContext> sacOp)
        {
            return InvokeAsync(() =>
            {
                sacOp(_sac);
            });
        }

        public Task InvokeAsync(Action action)
        {
            var task = new Task(action, cts.Token);
            _vmsJobs.Add(task);
            return task;
        }

        public void Invoke(Action action)
        {
            var task = new Task(action, cts.Token);
            _vmsJobs.Add(task);
            try
            {
                task.GetAwaiter().GetResult();
                if (task.IsFaulted)
                {
                    throw task.Exception;
                }
            }
            catch (Exception ex)
            {
                NonVMSInvoke(() =>
                {
                    _logger.Error(ex);
                    OnExceptionRaisedHandler(ex);
                });
                throw;
            }
        }

        private Task NonVMSInvoke(Action action)
        {
            var task = new Task(action);
            _nonVMSJobs.Add(task);
            return task;
        }

        public void DisposeVMS()
        {
            Invoke(new Action(() =>
            {
                if (_sac != null)
                {
                    _sac.Application?.Dispose();
                    _sac = null;
                }
            }));
        }

        public void Dispose()
        {
            DisposeVMS();
            if (!_vmsJobs.IsAddingCompleted)
            {
                while (_vmsJobs.Count > 0)
                {
                    Task item;
                    _vmsJobs.TryTake(out item);
                }
                _vmsJobs.CompleteAdding();
                _nonVMSJobs.CompleteAdding();
            }
        }

        public int ThreadId => vmsThread.ManagedThreadId;

        public delegate void ExceptionRaisedHandler(Exception ex);
        public event ExceptionRaisedHandler ExceptionRaised;

        public void OnExceptionRaisedHandler(Exception ex)
        {
            ExceptionRaised?.Invoke(ex);
        }

        public event EventHandler ContextLoaded;

        public void RaiseContextLoaded()
        {
            ContextLoaded?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ThreadStopping;

        public void RaiseThreadStopping()
        {
            ThreadStopping?.Invoke(this, EventArgs.Empty);
        }
    }
}
