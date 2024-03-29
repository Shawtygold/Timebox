﻿using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;

namespace Timebox.MVVM.Model
{
    internal class Alarm : ObservableObject
    {
        #region [Database properties]
        
        public ulong Id { get; set; }
        public string Description { get; set; } = null!;
        public string TriggerTime { get; set; } = null!;

        private string _soundSource = null!;
        public string SoundSource
        {
            get { return _soundSource; }
            private set { _soundSource = value; OnPropertyChanged(); }
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

        private bool _removeAfterTriggering;
        public bool RemoveAfterTriggering
        {
            get { return _removeAfterTriggering; }
            set { _removeAfterTriggering = value; OnPropertyChanged(); }
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

        public Alarm(string description, string triggerTime, string soundSource, bool isRepeat, bool isEnabled, bool removeAfterTriggering)
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Uncheck);

            Description = description;
            TriggerTime = triggerTime;
            SoundSource = soundSource;
            IsRepeat = isRepeat;
            IsEnabled = isEnabled;
            RemoveAfterTriggering = removeAfterTriggering;
        }

        public Alarm(ulong id, string description, string triggerTime, string soundSource, bool isRepeat, bool isEnabled, bool removeAfterTriggering)
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Uncheck);

            Id = id;
            Description = description;
            TriggerTime = triggerTime;
            SoundSource = soundSource;
            IsRepeat = isRepeat;
            IsEnabled = isEnabled;
            RemoveAfterTriggering = removeAfterTriggering;
        } 

        #region [Commands]

        [NotMapped] public ICommand CheckedCommand { get; set; }
        [NotMapped] public ICommand UncheckedCommand { get; set; }

        private void Checked(object? obj) => Start();
        private void Uncheck(object obj) => Stop();

        #endregion

        #region [Events]
        
        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            // Notification
            var notify = new ToastContentBuilder();
            notify.AddAppLogoOverride(new Uri(@"C:\Users\user\source\repos\Timebox\Timebox\Resources\AlarmIconWithBackground.png"), ToastGenericAppLogoCrop.Circle); //!!!!!! CHANGE ICON
            notify.SetToastScenario(ToastScenario.Reminder);
            notify.AddArgument("action", "ALARM_NOTIFICATION_CLICK");
            //notify.AddArgument("conversationId", 10000);
            notify.AddText($"Alarm {TriggerTime}");
            notify.AddText($"{Description}");
            notify.AddButton(new ToastButton().SetContent("Ok").AddArgument("action", "ALARM_OK"));
            notify.AddAudio(new Uri(SoundSource), true);
            notify.Show();           

            if (timer == null)
                return;

            if (RemoveAfterTriggering)
            {
                await AlarmDatabase.RemoveAlarm(this);
                return;
            }
            else
            {
                if (!timer.AutoReset)
                    Stop();
                else RestartTimer();
            }
        }
        #endregion

        #region [Methods]
        private double GetIntervalInMillisecond()
        {
            DateTime triggerTime = new();
            try
            {
                triggerTime = DateTime.Parse(TriggerTime);
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message); };

            if (triggerTime < DateTime.Now)
                triggerTime = triggerTime.AddDays(1);

            return Math.Round((triggerTime - DateTime.Now).TotalMilliseconds, 0);
        }
        private async void Start()
        {
            if (!IsEnabled)
            {
                StartTimer();
                IsEnabled = true;
                await AlarmDatabase.EditAlarm(this); //edit IsEnabled field in the databse
            }
        }
        private async void Stop()
        {
            if (IsEnabled)
            {
                StopTimer();
                IsEnabled = false;
                await AlarmDatabase.EditAlarm(this); //edit IsEnabled field in the databse
            }
        }
        private void RestartTimer()
        {
            if (timer == null)
                return;

            timer.Interval = (double)(86400) * 1000;
        }
        public void StartTimer()
        {
            timer = new(GetIntervalInMillisecond());
            timer.AutoReset = IsRepeat;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        public void StopTimer()
        {
            if (timer == null || !timer.Enabled)
                return;

            timer.Stop();
            timer.Dispose();
        }

        #endregion
    }
}
