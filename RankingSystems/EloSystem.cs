using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class EloSystem
    {
        public EloSystem(double k)
        {
            this.K = k;
        }

        public double K { get; }
    }
}
