using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class TimeRange
    {
        public double RelativeStartTime { get; }

        public double RelativeEndTime { get; }

        public AbsoluteTimeRange Offset { get; }

        public TimeRange(double rstart, double rend, AbsoluteTimeRange offset)
        {
            RelativeStartTime = rstart;
            RelativeEndTime = rend;
            Offset = offset;
        }

        public bool Contains(TimePoint time)
        {
            var currentRange = time.AbsoluteTimeOffset.TimeOffset;
            var relative = time.AbsoluteTimeOffset.TimeOffset - Offset.StartTime;
            return Offset.Contains(time.AbsoluteTimeOffset) && relative >= RelativeStartTime && relative <= RelativeEndTime;
        }

        public override string ToString()
        {
            return $"[{RelativeStartTime:0.00}...{RelativeEndTime:0.00}]";
        }
    }
}
