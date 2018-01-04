using AwesomenautsReplayParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser
{
    public class ReplayBase
    {
        public AbsoluteTimeRange BaseRange { get; protected set; }

        private static readonly int timeSize = 9;

        public static void ReadHeader(BitStream b, int secondSize = 5)
        {
            bool v = b.ReadBool();
            b.ReadUInt(v ? 8 : 18).ToString("X");
            b.ReadInt(secondSize).ToString("X");
        }

        public Vot<T> ReadVotList<T>(BitStream stream, int countSize, Func<T> read)
        {
            //Console.WriteLine($"VotList({typeof(T).Name}):");

            var list = new Vot<T>();

            bool exists = stream.ReadBool();
            if (exists)
            {
                uint count = stream.ReadUInt(countSize);
                var lastTime = new TimePoint(-1, BaseRange);

                for (var i = 0; i < count; i++)
                {
                    var time = ReadTimePoint(stream);
                    var value = read();

                    if (time < lastTime)
                    {
                        throw new FormatException("Invalid parsing structure: time not ascending. This usually indicates that you've specified the wrong field size somewhere (either in this call or the previous).");
                    }

                    lastTime = time;

                    //Console.WriteLine($"    - {time:00.00}: {value}");
                    list.Add(time, value);
                }
            }

            return list;
        }

        public Vot<double> ReadFloatVotList(BitStream stream, int countSize, int valueSize, double minValue, double maxValue)
            => ReadVotList(stream, countSize, () => stream.ReadFloat(valueSize, minValue, maxValue));

        public Vot<(double f1, double f2)> ReadFloatTupleVotList(BitStream stream, int countSize, int valueSize, double minValue1, double maxValue1, double minValue2, double maxValue2)
            => ReadVotList(stream, countSize, () =>
            {
                var x = stream.ReadFloat(valueSize, minValue1, maxValue1);
                var y = stream.ReadFloat(valueSize, minValue2, maxValue2);
                return (x, y);
            });

        public Vot<Position> ReadPositionVotList(BitStream stream, int countSize, int valueSize)
            => ReadVotList(stream, countSize, () =>
            {
                return ReadPosition(stream, valueSize);
            });

        public Position ReadPosition(BitStream stream, int valueSize)
        {
            var x = stream.ReadFloat(valueSize, -10, 20);
            var y = stream.ReadFloat(valueSize, -10, 30);
            return new Position(x, y);
        }

        public Vot<uint> ReadUIntVotList(BitStream stream, int countSize, int valueSize)
            => ReadVotList(stream, countSize, () => stream.ReadUInt(valueSize));

        public Vot<int> ReadIntVotList(BitStream stream, int countSize, int valueSize)
            => ReadVotList(stream, countSize, () => stream.ReadInt(valueSize));

        public bool ReadVoidVotList(BitStream stream, int countSize)
        {
            //Console.WriteLine("VotList:");

            bool countIsZero = stream.ReadBool();
            uint count = stream.ReadUInt(countSize);
            var lastTime = new TimePoint(-1, BaseRange);

            for (var i = 0; i < count; i++)
            {
                var time = ReadTimePoint(stream);

                if (time < lastTime)
                {
                    throw new FormatException("Invalid parsing structure: time not ascending. This usually indicates that you've specified the wrong field size somewhere (either in this call or the previous).");
                }

                lastTime = time;

                //Console.WriteLine($"    - @{time:00.00}");
            }

            return countIsZero || count >= 1;
        }

        public TimePoint ReadTimePoint(BitStream stream) => new TimePoint(stream.ReadFloat(timeSize, 0.0, 10.5), BaseRange);

        public TimeRange ReadTimeRange(BitStream b)
        {
            var start = b.ReadFloat(9, 0, 10.5);
            var end = b.ReadFloat(9, 0, 10.5);

            var timeRange = new TimeRange(start, end, BaseRange);
            //Console.WriteLine(timeRange);

            return timeRange;
        }
    }
}
