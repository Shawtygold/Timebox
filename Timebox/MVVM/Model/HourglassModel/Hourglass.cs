using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using System.Windows;
using Timebox.Core;

namespace Timebox.MVVM.Model.HourglassModel
{
    internal class Hourglass : ObservableObject
    {
        public ulong Id { get; set; }
        public string Description { get; set; } = null!;

        private System.Timers.Timer _timer;
        //private static SoundPlayer _soundPlayer;

        public int Interval { get; set; } // Contains the maximum value of hourglass (In seconds)
        [NotMapped] public int RemainingTime { get; set; } // (In seconds)

        public delegate void HourglassElapsedHandler(object sender, HourglassElapsedEventArgs e);
        public event HourglassElapsedHandler? Elapsed;

        public delegate void HourglassChangedHandler(object sender, HourglassChangedEventArgs e);
        public event HourglassChangedHandler RemainingTimeChanged;

        public Hourglass()
        {        
            Interval = 60;
            RemainingTime = Interval;
            Description = "";

            this.Elapsed += Hourglass_Elapsed;

            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        public Hourglass(string description, int interval)
        {
            if (interval <= 0)
            {
                MessageBox.Show("The interval cannot be less than or equal to zero");
                return;
            }
            
            Interval = interval;
            RemainingTime = Interval;

            Description = description;

            this.Elapsed += Hourglass_Elapsed;

            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        private void OnRemainingTimeChanged(HourglassChangedEventArgs e) => RemainingTimeChanged?.Invoke(this, e);
        private void OnHourglassElapsed(HourglassElapsedEventArgs e) => Elapsed?.Invoke(this, e);

        public void Start()
        {
            if (_timer == null || _timer.Enabled)
                return;

            if (RemainingTime == 0)
                RemainingTime = Interval;

            _timer.AutoReset = true;
            _timer.Start();
            OnRemainingTimeChanged(new HourglassChangedEventArgs(RemainingTime));
        }
        public void Stop()
        {
            if (_timer == null || _timer.Enabled == false)
                return;

            _timer.Stop();
        }
        public void ChangeInterval(int interval)
        {
            if (interval <= 0)
            {
                MessageBox.Show("The interval cannot be less than or equal to zero");
                return;
            }

            RemainingTime = interval;
            Interval = interval;
        }
        //public static void StopSound()
        //{
        //    if (_soundPlayer == null)
        //        return;

        //    _soundPlayer.Stop();
        //}

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

            // Notification
            var notify = new ToastContentBuilder();
            notify.AddAppLogoOverride(new Uri(@"C:\Users\user\source\repos\Timebox\Timebox\Resources\AlarmIconWithBackground.png"), ToastGenericAppLogoCrop.Circle);
            notify.SetToastScenario(ToastScenario.Reminder);
            notify.AddArgument("action", "HOURGLASS_NOTIFICATION_CLICK");
            //notify.AddArgument("conversationId", 9813);
            notify.AddAudio(new Uri("ms-winsoundevent:Notification.Looping.Alarm3"), true);
            notify.AddText("Hourglass");
            notify.AddText($"{Converter.ConvertSecondsToTime(hourglass.Interval)}");
            notify.AddText($"{Description}");
            notify.AddButton(new ToastButton().SetContent("Ok").AddArgument("action", "HOURGLASS_OK"));
            notify.Show();

            // Sound (Решил на неопределенный срок убрать данный функционал, поскольку звуки, которые воспроизводит уведомление кажутся лучше)
            //try
            //{
            //    _soundPlayer = new(@$"C:\Users\user\Downloads\alarm09.wav");
            //    _soundPlayer.PlayLooping();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}
