using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel.DataAnnotations.Schema;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;

namespace Timebox.MVVM.Model
{
    public class Alarm : ObservableObject
    {
        #region [Database properties]

        public ulong Id { get; set; }
        public string Description { get; set; } = null!;
        public string TriggerTime { get; set; } = null!;

        private string _soundSource;
        public string SoundSource
        {
            get { return _soundSource; }
            set { _soundSource = value; OnPropertyChanged(); }
        }

        private bool _isRepeat;
        public bool IsRepeat
        {
            get { return _isRepeat; }
            set { _isRepeat = value; if (_isRepeat == true) Repeat = "everyday"; else { Repeat = "onlyone"; } OnPropertyChanged(); }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; if (_isEnabled == true){ TextOpacity = 1; IsChecked = true; } else { TextOpacity = 0.7; if (IsChecked) IsChecked = false; } OnPropertyChanged(); }
        }

        #endregion

        #region [Non-database properties]

        private System.Timers.Timer? timer;

        private bool _isChecked = false;
        [NotMapped]
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(); }
        }

        private double _textOpacity = 0.7;
        [NotMapped]
        public double TextOpacity
        {
            get { return _textOpacity; }
            set { _textOpacity = value; OnPropertyChanged(); }
        }

        private string _repeat = "";
        [NotMapped]
        public string Repeat
        {
            get { return _repeat; }
            set { _repeat = value; OnPropertyChanged(); }
        }

        #endregion

        public Alarm(string description, string triggerTime, string soundSource, bool isRepeat, bool isEnabled)
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Uncheck);

            Description = description;
            TriggerTime = triggerTime;
            SoundSource = soundSource;
            IsRepeat = isRepeat;
            IsEnabled = isEnabled;
        }

        public Alarm(ulong id, string description, string triggerTime, string soundSource, bool isRepeat, bool isEnabled)
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Uncheck);

            Id = id;
            Description = description;
            TriggerTime = triggerTime;
            SoundSource = soundSource;
            IsRepeat = isRepeat;
            IsEnabled = isEnabled;         
        } 

        #region [Commands]

        [NotMapped] public ICommand CheckedCommand { get; set; }
        [NotMapped] public ICommand UncheckedCommand { get; set; }

        private void Checked(object? obj) => Start();
        private void Uncheck(object obj) => Stop();

        #endregion

        #region [Events]

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            var notify = new ToastContentBuilder();
            notify.AddText($"{Description}");
            
            notify.Show();

            try
            {
                SoundPlayer simpleSound = new(@$"{SoundSource}");
                simpleSound.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Stop();
            }

            if (timer == null)
                return;

            if (!timer.AutoReset)
                Stop();          
            else RestartTimer();
        }
        #endregion

        #region [Methods]

        private int GetIntervalInSeconds()
        {
            DateTime triggerTime = new();
            try
            {
                triggerTime = DateTime.Parse(TriggerTime);
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message); };

            if (triggerTime < DateTime.Now)
                triggerTime = triggerTime.AddDays(1);

            return (int)Math.Round((triggerTime - DateTime.Now).TotalSeconds, 0);
        }
        private async void Start()
        {
            if (!IsEnabled)
            {
                StartTimer();
                IsEnabled = true;
                await Database.EditAlarm(this); //изменение поля IsEnabled в бд
            }
        }
        private async void Stop()
        {
            if (IsEnabled)
            {
                StopTimer();
                IsEnabled = false;
                await Database.EditAlarm(this); //изменение поля IsEnabled в бд
            }
        }
        private void RestartTimer()
        {
            if (timer == null) //LOGIC
                return;

            timer.Interval = (double)(86400)/* * 1000*/;
        }

        internal void StartTimer()
        {
            timer = new(GetIntervalInSeconds());
            timer.AutoReset = IsRepeat;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        internal void StopTimer()
        {
            if (timer == null)
                return;

            timer.Stop();
            timer.Dispose();
        }

        #endregion
    }
}
