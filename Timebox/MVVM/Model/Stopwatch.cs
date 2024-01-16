namespace Timebox.MVVM.Model
{
    internal class Stopwatch
    {
        public delegate void SecondsChangedHandler(object sender, StopwatchSecondsChangedEventArgs e);
        public event SecondsChangedHandler? PassedSecondsChanged;

        private void OnPassedSecondsChanged(StopwatchSecondsChangedEventArgs e) => PassedSecondsChanged?.Invoke(this, e);

        public Stopwatch()
        {
            _timer = new(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            PassedSeconds++;
            OnPassedSecondsChanged(new StopwatchSecondsChangedEventArgs(PassedSeconds));
        }

        #region [Properties]

        private readonly System.Timers.Timer? _timer;
        private int PassedSeconds { get; set; }

        #endregion

        public void Stop()
        {
            if (_timer == null)
                return;

            _timer.Stop();
        }

        public void Start()
        {
            if (_timer == null)
                return;

            _timer.Start();
        }

        public void Reset()
        {
            PassedSeconds = 0;
            OnPassedSecondsChanged(new StopwatchSecondsChangedEventArgs(PassedSeconds));
        }
    }
}
