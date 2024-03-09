using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContourChecker.UI.Models;

namespace ContourChecker.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            //Generate fake list of contour checks
            for (int i = 0; i < 10; i++)
            {
                ContourChecks.Add(new UIContourCheck()
                {
                    Name = $"Contour Check {i}",
                    IsPassed = i % 2 == 0
                });
            }
        }

        public ObservableCollection<UIContourCheck> ContourChecks { get; } = new ObservableCollection<UIContourCheck>();
    }
}
