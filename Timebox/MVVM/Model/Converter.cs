using System.Windows;

namespace Timebox.MVVM.Model
{
    internal class Converter
    {
        public static string ConvertSecondsToTime(int interval)
        {
            int hours = interval / 3600;
            int minutes = (interval % 3600) / 60;
            int seconds = (interval % 3600) % 60;

            string strHours = hours.ToString();
            string strMinutes = minutes.ToString();
            string strSeconds = seconds.ToString();

            if (seconds < 10)
                strSeconds = "0" + strSeconds;
            if (minutes < 10)
                strMinutes = "0" + strMinutes;
            if (hours < 10)
                strHours = "0" + strHours;

            return $"{strHours}:{strMinutes}:{strSeconds}";
        }

        public static int? ConvertTimeToSeconds(string time)
        {
            if(time.Length != 8)
                return null;

            if (time[2] != ':' || time[5] != ':')
                return null;

            List<string> times = time.Split(':').ToList();

            if(times.Count != 3)
                return null;

            int hours;
            int minutes;
            int seconds;

            try
            {
                hours = int.Parse(times[0]);
                minutes = int.Parse(times[1]);
                seconds = int.Parse(times[2]);
            }
            catch
            {
                return null;
            }

            int result = (hours * 3600) + (minutes * 60) + seconds;
            return result;
        }
    }
}
