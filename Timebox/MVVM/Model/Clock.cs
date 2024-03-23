namespace Timebox.MVVM.Model
{
    internal class Clock : Core.ObservableObject
    {
        public Clock()
        {
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToShortTimeString();

            _timer = new(1000);
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            Icon = GetIconTimeOfDay(DateTime.Now);
        }

        #region [Events]

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {        
            DateTime now = DateTime.Now;
            Time = now.ToShortTimeString();
            Icon = GetIconTimeOfDay(now);
        }

        #endregion

        #region [Properties]

        private readonly System.Timers.Timer _timer;

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged(); }
        }

        private string _time = null!;
        public string Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(); }
        }

        private string _date = null!;
        public string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Methods]

        private static string GetIconTimeOfDay(DateTime dateTimeNow)
        {
            double hours = Math.Round(dateTimeNow.TimeOfDay.TotalHours, 0);

            if (hours < 18)
                return "Sun";

            return "Moon";           
        }

        #endregion
    }
}
