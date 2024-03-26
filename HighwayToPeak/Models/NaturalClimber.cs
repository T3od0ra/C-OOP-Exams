using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public class NaturalClimber : Climber
    {
        private const int startingStamina = 6;
        private const int staminaRecovery = 2;


        public NaturalClimber(string name) : base(name, startingStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount * staminaRecovery;
        }
    }
}
