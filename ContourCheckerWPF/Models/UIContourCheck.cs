using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourChecker.UI.Models
{
    public partial class UIContourCheck : ObservableObject
    {
        [ObservableProperty]
        private bool isPassed;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private double progress;

        [RelayCommand]
        public void FixAction()
        {

        }
        
    }
}
