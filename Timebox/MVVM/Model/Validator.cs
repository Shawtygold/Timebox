namespace Timebox.MVVM.Model
{
    internal class Validator
    {
        internal static string ValidateAlarm(int hours, int minutes)
        {
            string exceptionMsg = "";

            if((hours > 23 || hours < 0) || (minutes > 59 || minutes < 0))
                exceptionMsg += "Incorrect time entry.\n";

            if (exceptionMsg.Trim() != "")
                return exceptionMsg;
            else
                return "OK";
        }

        internal static string ValidateHourglass(int hours, int minutes, int seconds)
        {
            string exceptionMsg = "";

            if ((hours > 99 || hours < 0) || (minutes > 59 || minutes < 0)  || (seconds > 59 || seconds < 0))
                exceptionMsg += "Incorrect time entry.\n";

            if (exceptionMsg.Trim() != "")
                return exceptionMsg;
            else
                return "OK";
        }
    }
}
