using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.MVVM.Model.HourglassModel;
using Timebox.Services;

namespace Timebox.MVVM.ViewModel
{
    internal class HourglassViewModel : Core.ViewModel
    {
        public HourglassViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);

            _hourglass = new();
            _hourglass.RemainingTimeChanged += Hourglass_RemainingTimeChanged;
            _hourglass.Elapsed += Hourglass_Elapsed;
            RemainingTime = Converter.ConvertSecondsToTime(_hourglass.Interval);
        }

        private void Hourglass_Elapsed(object sender, HourglassElapsedEventArgs e)
        {
            ProgressRingColor = "#303030";
        }

        private void Hourglass_RemainingTimeChanged(object sender, HourglassChangedEventArgs e)
        {
            if(_hourglass == null)
                return;

            ProgressRingColor = "#6D79FF";

            int remainingTime = e.RemainingTime;

            RemainingTime = Converter.ConvertSecondsToTime(remainingTime);
            Progress = GetPercent(_hourglass.Interval, remainingTime);
        }

        #region [Properties]

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged(); }
        }

        private string _progressRingColor = "#303030";
        public string ProgressRingColor
        {
            get { return _progressRingColor; }
            set { _progressRingColor = value; OnPropertyChanged(); }
        }


        private readonly Hourglass? _hourglass;

        private string _remainingTime = "0";
        public string RemainingTime
        {
            get { return _remainingTime; }
            set { _remainingTime = value; OnPropertyChanged(); }
        }

        private int? IntervalInSeconds { get; set; }

        private string _hourglassTime;
        public string HourglassTime
        {
            get { return _hourglassTime; }
            set { _hourglassTime = value; IntervalInSeconds = Converter.ConvertTimeToSeconds(_hourglassTime); if (IntervalInSeconds == null) { MessageBox.Show("Enter the time in the format hh:mm:ss (01:30:59)"); return; } if (_hourglass != null) _hourglass.ChangeInterval((int)IntervalInSeconds); OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }

        private void Start(object obj)
        {
            if (_hourglass == null || IntervalInSeconds == null)
                return;

            _hourglass.Start();
        }
        private void Stop(object obj)
        {
            if (_hourglass == null)
                return;

            _hourglass.Stop();
        }

        #endregion

        private static double GetPercent(double maxValue, double value)
        {
            if (maxValue == 0 || value == 0)
                return 0;

            double percent = (value * 100) / maxValue;

            return Math.Round(percent, 0);
        }
    }
}
