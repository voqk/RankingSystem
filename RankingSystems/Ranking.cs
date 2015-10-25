using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class Ranking
    {
        public Ranking(double value, DateTimeOffset timeStamp)
        {
            this.Value = value;
            this.TimeStamp = timeStamp;
        }

        public double Value { get; }
        public DateTimeOffset TimeStamp { get; }
    }
}
