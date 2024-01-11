using System.Collections.ObjectModel;

namespace Timebox.MVVM.Model
{
    internal class AlarmHelper
    {
        internal static void AlarmStart(ObservableCollection<Alarm>? alarms)
        {
            if (alarms == null || alarms.Count == 0)
                return;

            for (int i = 0; i < alarms.Count; i++)
            {
                if (alarms[i].IsEnabled == true)
                {
                    alarms[i].StartTimer();
                }
            }
        }
    }
}
