namespace Timebox.MVVM.Model
{
    internal class DatabaseChangedEventArgs
    {
        public object? ChangedElement { get; set; } // Contains an added or modified object (alarm or hourglass)
        public string Action { get; set; }
        public DatabaseChangedEventArgs(string action, object? changedElement)
        {
            Action = action;
            ChangedElement = changedElement;
        }
    }
}
