using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.MVVM.Model
{
    internal class HourglassChangedEventArgs
    {
        public int RemainingTime { get; set; }
        public HourglassChangedEventArgs(int remainingTime)
        {
            RemainingTime = remainingTime;
        }
    }
}
