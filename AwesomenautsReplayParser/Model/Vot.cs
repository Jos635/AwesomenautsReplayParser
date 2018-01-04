using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomenautsReplayParser.Model
{
    public class Vot<T> : IEnumerable<T>, IEnumerable<(TimePoint, T)>
    {
        private List<(TimePoint time, T item)> times = new List<(TimePoint time, T)>();

        public T this[TimePoint point]
        {
            get
            {
                int minIndex = 0, maxIndex = times.Count;

                while(times.Count > 0)
                {
                    var pos = (minIndex + maxIndex) / 2;
                    (TimePoint time, T item) item = times[pos];
                    if (pos == minIndex || pos == maxIndex)
                    {
                        return item.item;
                    }
                    else
                    {
                        if (point < item.time)
                        {
                            maxIndex = pos;
                        }
                        else if (point > item.time)
                        {
                            minIndex = pos;
                        }
                        else
                        {
                            return item.item;
                        }
                    }
                }

                return default(T);
            }
        }

        public void Add(TimePoint time, T item)
        {
            times.Add((time, item));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return times.Select(item => item.item).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return times.Select(item => item.item).GetEnumerator();
        }

        IEnumerator<(TimePoint, T)> IEnumerable<(TimePoint, T)>.GetEnumerator()
        {
            return times.GetEnumerator();
        }
    }
}
