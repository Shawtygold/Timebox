using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel.DataAnnotations.Schema;
using System.DirectoryServices.ActiveDirectory;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;

namespace Timebox.MVVM.Model
{
    internal class Hourglass : ObservableObject
    {
        #region [Database properties]

        public ulong Id { get; set; }
        public string Description { get; set; } = null!;
        public int Interval { get; set; } // Contains the maximum value of hourglass (In seconds)

        #endregion

        #region [Non-database properties]

        private System.Timers.Timer _timer;
        
        [NotMapped] private bool IsActive { get; set; } = false;
        [NotMapped] private int RemainingTime { get; set; } // (In seconds)

        private string _strRemainingTime;
        [NotMapped]
        public string StrRemainingTime
        {
            get { return _strRemainingTime; }
            set { _strRemainingTime = value; OnPropertyChanged(); }
        }

        private string _progressRingColor;
        [NotMapped]
        public string ProgressRingColor
        {
            get { return _progressRingColor; }
            set { _progressRingColor = value; OnPropertyChanged(); }
        }

        private double _progress;
        [NotMapped]
        public double Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged(); }
        }

        #endregion

        public Hourglass(ulong id, string description, int interval)
        {
            Id = id;
            Interval = interval;
            RemainingTime = Interval;
            StrRemainingTime = Converter.ConvertSecondsToTime(RemainingTime);

            Description = description;

            StartCommand = new RelayCommand(Start1);
            StopCommand = new RelayCommand(Stop1);
            ResetCommand = new RelayCommand(Reset1);

            Elapsed += Hourglass_Elapsed;
            RemainingTimeChanged += Hourglass_RemainingTimeChanged;

            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        public Hourglass(string description, int interval)
        {
            Interval = interval;
            RemainingTime = Interval;
            StrRemainingTime = Converter.ConvertSecondsToTime(RemainingTime);

            Description = description;

            StartCommand = new RelayCommand(Start1);
            StopCommand = new RelayCommand(Stop1);
            ResetCommand = new RelayCommand(Reset1);

            Elapsed += Hourglass_Elapsed;
            RemainingTimeChanged += Hourglass_RemainingTimeChanged;

            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        #region [Events]

        public delegate void ElapsedHandler(object sender, HourglassElapsedEventArgs e);
        public event ElapsedHandler? Elapsed;

        public delegate void ChangedHandler(object sender, HourglassChangedEventArgs e);
        public event ChangedHandler RemainingTimeChanged;

        private void Hourglass_RemainingTimeChanged(object sender, HourglassChangedEventArgs e)
        {
            ProgressRingColor = "#6D79FF";

            int remainingTime = e.RemainingTime;

            StrRemainingTime = Converter.ConvertSecondsToTime(remainingTime);
            Progress = GetPercent(Interval, remainingTime);
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            RemainingTime--;
            OnRemainingTimeChanged(new HourglassChangedEventArgs(RemainingTime));

            if (RemainingTime == 0)
            {
                _timer.AutoReset = false;
                _timer.Enabled = false;
                OnHourglassElapsed(new HourglassElapsedEventArgs(DateTime.Now));
            }
        }

        private void Hourglass_Elapsed(object sender, HourglassElapsedEventArgs e)
        {
            if (sender is not Hourglass hourglass)
                return;

            ProgressRingColor = "Transparent";

            // Notification
            var notify = new ToastContentBuilder();
            notify.AddAppLogoOverride(new Uri(@"C:\Users\user\Pictures\Figma\HourglassWithBackground.png"), ToastGenericAppLogoCrop.Circle);
            notify.SetToastScenario(ToastScenario.Reminder);
            notify.AddArgument("action", "HOURGLASS_NOTIFICATION_CLICK");
            //notify.AddArgument("conversationId", 9813);
            notify.AddAudio(new Uri("ms-winsoundevent:Notification.Looping.Alarm3"), true);
            notify.AddText("Hourglass");
            notify.AddText($"{Converter.ConvertSecondsToTime(hourglass.Interval)}");
            notify.AddText($"{Description}");
            notify.AddButton(new ToastButton().SetContent("Ok").AddArgument("action", "HOURGLASS_OK"));
            notify.Show();
        }

        #endregion

        #region [Commands]

        [NotMapped] public ICommand StartCommand { get; set; }
        [NotMapped] public ICommand StopCommand { get; set; }
        [NotMapped] public ICommand ResetCommand { get; set; }

        private void Start1(object obj) => Start();
        private void Stop1(object obj) => Stop();
        private void Reset1(object obj) => Reset();

        #endregion

        #region [Methods]

        private void OnRemainingTimeChanged(HourglassChangedEventArgs e) => RemainingTimeChanged?.Invoke(this, e);
        private void OnHourglassElapsed(HourglassElapsedEventArgs e) => Elapsed?.Invoke(this, e);

        private void Start()
        {
            if (_timer == null || _timer.Enabled)
                return;

            if (RemainingTime == 0)
                RemainingTime = Interval;

            _timer.Start();
            IsActive = true;
            OnRemainingTimeChanged(new HourglassChangedEventArgs(RemainingTime));
        }
        private void Stop()
        {
            if (_timer == null || _timer.Enabled == false)
                return;

            _timer.Stop();

            IsActive = false;
        }
        private void Reset()
        {
            if (_timer == null)
                return;

            RemainingTime = Interval;
            OnRemainingTimeChanged(new HourglassChangedEventArgs(RemainingTime));

            if (IsActive)
                return;

            ProgressRingColor = "Transparent";
        }
        private static double GetPercent(double maxValue, double value)
        {
            if (maxValue == 0 || value == 0)
                return 0;

            double percent = value * 100 / maxValue;

            return Math.Round(percent, 0);
        }

        #endregion
    }
}
