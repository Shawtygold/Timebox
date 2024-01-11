namespace Timebox.MVVM.Model
{
    internal class HourglassChangedEventArgs
    {
        public int RemainingTime { get; set; }
        public HourglassChangedEventArgs(int remainingTime)
        {
            RemainingTime = remainingTime;
        }
    }
}
