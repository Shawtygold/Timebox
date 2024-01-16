using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.Services;

namespace Timebox.MVVM.ViewModel
{
    internal class StopwatchViewModel : Core.ViewModel
    {
        public StopwatchViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ResetCommand = new RelayCommand(Reset);

            _stopwatch = new();
            _stopwatch.PassedSecondsChanged += Stopwatch_PassedSecondsChanged;
        }

        private void Stopwatch_PassedSecondsChanged(object sender, StopwatchSecondsChangedEventArgs e)
        {
            int passedSeconds = e.PassedSeconds;
            StrPassedSeconds = Converter.ConvertSecondsToTime(passedSeconds);
        }

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private string _strPassedSeconds = "00:00:00";
        public string StrPassedSeconds
        {
            get { return _strPassedSeconds; }
            set { _strPassedSeconds = value; OnPropertyChanged(); }
        }

        private readonly Stopwatch _stopwatch;

        #region [Commands]

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        private void Start(object obj)
        {
            if (_stopwatch == null)
                return;

            _stopwatch.Start();
        }

        private void Stop(object obj)
        {
            if (_stopwatch == null)
                return;

            _stopwatch.Stop();
        }

        private void Reset(object obj)
        {
            if (_stopwatch == null)
                return;

            _stopwatch.Reset();
        }

        #endregion
    }
}
