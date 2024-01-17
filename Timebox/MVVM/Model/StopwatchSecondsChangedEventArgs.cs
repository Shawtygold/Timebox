namespace Timebox.MVVM.Model
{
    internal class StopwatchSecondsChangedEventArgs
    {
        public int PassedSeconds { get; set; }

        public StopwatchSecondsChangedEventArgs(int passedSeconds)
        {
            PassedSeconds = passedSeconds;
        }
    }
}
