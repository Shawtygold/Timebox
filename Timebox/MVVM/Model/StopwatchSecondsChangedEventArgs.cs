using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.MVVM.Model
{
    internal class StopwatchSecondsChangedEventArgs
    {
        public int PassedSeconds { get; set; }

        public StopwatchSecondsChangedEventArgs(int passedSeconds)
        {
            PassedSeconds = passedSeconds;
        }
    }
}
