using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class TimePoint
    {
        public double RelativeTimeOffset { get; }

        public AbsoluteTimeRange BaseOffset { get; private set; }

        public AbsoluteTime AbsoluteTimeOffset => BaseOffset + RelativeTimeOffset;

        public TimePoint(double relativeTimeOffset, AbsoluteTimeRange offset)
        {
            RelativeTimeOffset = relativeTimeOffset;
            BaseOffset = offset;
        }

        public static bool operator >(TimePoint a, TimePoint b)
        {
            return a.RelativeTimeOffset > b.RelativeTimeOffset;
        }

        public static bool operator <(TimePoint a, TimePoint b)
        {
            return a.RelativeTimeOffset < b.RelativeTimeOffset;
        }
        
        public static bool operator ==(TimePoint a, TimePoint b)
        {
            return a.RelativeTimeOffset == b.RelativeTimeOffset;
        }

        public static bool operator !=(TimePoint a, TimePoint b)
        {
            return a.RelativeTimeOffset > b.RelativeTimeOffset;
        }

        public override int GetHashCode()
        {
            return RelativeTimeOffset.GetHashCode() + AbsoluteTimeOffset.GetHashCode() * 21;
        }

        public override bool Equals(object obj)
        {
            if(obj is TimePoint tp)
            {
                return tp.RelativeTimeOffset == RelativeTimeOffset;
            }

            return false;
        }

        public override string ToString()
        {
            return $"@{RelativeTimeOffset:0.00}";
        }
    }
}
