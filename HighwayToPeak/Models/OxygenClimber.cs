using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public class OxygenClimber : Climber
    {
        private const int startingStamina = 10;
        private const int staminaRecovery = 1;
        public OxygenClimber(string name) : base(name, startingStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount * staminaRecovery;
        }
    }
}
