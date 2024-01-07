using System.IO;

namespace Timebox.MVVM.Model
{
    internal class Validator
    {
        internal static string ValidateAlarm(int hours, int minutes)
        {
            string exceptionMsg = "";

            if((minutes > 59 || minutes < 0) || (hours > 23 || hours < 0))
                exceptionMsg += "Incorrect time entry.\n";

            if (exceptionMsg.Trim() != "")
                return exceptionMsg;
            else
                return "OK";
        }

        internal static string ValidateHourglass(int hours, int minutes, int seconds)
        {
            string exceptionMsg = "";

            if ((minutes > 59 || minutes < 0) || (hours > 23 || hours < 0) || (seconds > 59 || seconds < 0))
                exceptionMsg += "Incorrect time entry.\n";

            if (exceptionMsg.Trim() != "")
                return exceptionMsg;
            else
                return "OK";
        }
    }
}
