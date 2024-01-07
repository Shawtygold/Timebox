namespace Timebox.MVVM.Model
{
    internal class DatabaseChangedEventArgs
    {
        public Alarm? ChangedElement { get; set; } // Contains an added or modified alarm
        public string Action { get; set; }
        public DatabaseChangedEventArgs(string action, Alarm? changedElement)
        {
            Action = action;
            ChangedElement = changedElement;
        }
    }
}
