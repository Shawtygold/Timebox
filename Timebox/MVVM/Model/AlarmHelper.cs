using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.MVVM.Model
{
    internal class AlarmHelper
    {
        internal static void AlarmStop(ObservableCollection<Alarm>? alarms)
        {
            if (alarms == null || alarms.Count == 0)
                return;

            for (int i = 0; i < alarms.Count; i++)
            {
                alarms[i].StopTimer();
            }
        }

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
