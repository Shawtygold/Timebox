using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.MVVM.Model
{
    internal class HourglassElapsedEventArgs
    {
        public DateTime SignalTime { get; set; }

        public HourglassElapsedEventArgs(DateTime signalTime)
        {
            SignalTime = signalTime;
        }
    }
}
