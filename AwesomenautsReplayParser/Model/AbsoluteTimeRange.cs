using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class AbsoluteTimeRange
    {
        public double StartTime { get; }

        public double EndTime { get; }
        
        public AbsoluteTimeRange(double rstart, double rend)
        {
            StartTime = rstart;
            EndTime = rend;
        }

        public bool Contains(AbsoluteTime time)
        {
            return time.TimeOffset >= StartTime && time.TimeOffset <= EndTime;
        }

        public static AbsoluteTime operator+(AbsoluteTimeRange range, double offset)
        {
            if(offset < 0)
            {
                return new AbsoluteTime(range.StartTime);
            }else if (offset > 10)
            {
                return new AbsoluteTime(range.EndTime);
            }
            else
            {
                return new AbsoluteTime(range.StartTime + offset);
            }
        }

        public override string ToString()
        {
            return $"[{StartTime:0.00}...{EndTime:0.00}]";
        }
    }
}
