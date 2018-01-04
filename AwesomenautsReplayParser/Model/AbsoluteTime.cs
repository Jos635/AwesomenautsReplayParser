using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public struct AbsoluteTime
    {
        public double TimeOffset { get; }

        public AbsoluteTime(double offset)
        {
            TimeOffset = offset;
        }

        public static bool operator >(AbsoluteTime a, AbsoluteTime b)
        {
            return a.TimeOffset > b.TimeOffset;
        }

        public static bool operator <(AbsoluteTime a, AbsoluteTime b)
        {
            return a.TimeOffset < b.TimeOffset;
        }

        public static bool operator ==(AbsoluteTime a, AbsoluteTime b)
        {
            return a.TimeOffset == b.TimeOffset;
        }

        public static bool operator !=(AbsoluteTime a, AbsoluteTime b)
        {
            return a.TimeOffset > b.TimeOffset;
        }

        public static AbsoluteTime operator +(AbsoluteTime a, AbsoluteTime b)
        {
            return new AbsoluteTime(a.TimeOffset + b.TimeOffset);
        }

        public override int GetHashCode()
        {
            return TimeOffset.GetHashCode();
        }

        public TimePoint RelativeTo(AbsoluteTimeRange baseRange)
        {
            return new TimePoint(TimeOffset - baseRange.StartTime, baseRange);
        }

        public override bool Equals(object obj)
        {
            if (obj is AbsoluteTime at)
            {
                return at.TimeOffset == TimeOffset;
            }else if(obj is TimePoint tp)
            {
                return tp.AbsoluteTimeOffset == this;
            }

            return false;
        }

        public override string ToString()
        {
            return $"@{TimeOffset:0.00}";
        }
    }
}
