using System.IO;

namespace Timebox.MVVM.Model
{
    internal class Validator
    {
        internal static string Validate(int hours, int minutes, string soundSource)
        {
            string exceptionMsg = "";

            if((minutes > 60 || minutes < 0) || (hours > 24 || hours < 0))
                exceptionMsg += "Incorrect time entry.\n";

            if (!File.Exists(soundSource))
                exceptionMsg += $"File \"{soundSource}\" not found.\n";
            else if (!soundSource.EndsWith(".wav"))
            {
                exceptionMsg += $"The file located at {soundSource} is not a valid wave file. Please select a file with the extension \".wav\".";
            }

            if (exceptionMsg.Trim() != "")
                return exceptionMsg;
            else
                return "OK";
        }
    }
}
