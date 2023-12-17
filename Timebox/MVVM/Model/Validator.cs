namespace Timebox.MVVM.Model
{
    internal class Validator
    {
        internal static string Validate(int hours, int minutes)
        {
            bool exception = false;
            string exceptionMsg = "Incorrect time entry!";

            if(hours > 24 || hours < 0)
                exception = true;
            if(minutes > 60 || minutes < 0)
                exception = true;

            if (exception)
                return exceptionMsg;
            else
                return "OK";
        }
    }
}
