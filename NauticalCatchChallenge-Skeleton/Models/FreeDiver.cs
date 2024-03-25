using NauticalCatchChallenge.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class FreeDiver : Diver
    {
        private const int oxygenLevel = 120;
        public FreeDiver(string name) : base(name, oxygenLevel)
        {
        }

        public override void Miss(int TimeToCatch)
        {
            int usedOxyden = (int)Math.Round(oxygenLevel * 0.6);
            base.OxygenLevel -= usedOxyden;
        }

        public override void RenewOxy()
        {
            base.OxygenLevel = oxygenLevel;
        }
    }
}
