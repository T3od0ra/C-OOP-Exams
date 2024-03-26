using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public abstract class Climber : IClimber
    {
        private string name;
        private int stamina;
        private List<string> conqueredPeaks;

        public Climber(string name, int stamina)
        {
            Name = name;
            Stamina = stamina;
            conqueredPeaks = new List<string>();
        }
        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Climber's name cannot be null or whitespace.");
                }
                name = value;
            }
        }

        public int Stamina
        {
            get => stamina;
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 10)
                {
                    value = 10;
                }
                stamina = value;
            }
        }

        public IReadOnlyCollection<string> ConqueredPeaks => conqueredPeaks;
        public void Climb(IPeak peak)
        {
            if (!conqueredPeaks.Contains(peak.Name))
            {
                conqueredPeaks.Add(peak.Name);
            }

            int decrease = 0;

            if (peak.DifficultyLevel == "Extreme")
            {
                decrease += 6;
            }
            else if (peak.DifficultyLevel == "Hard")
            {
                decrease += 4;
            }
            else if (peak.DifficultyLevel == "Moderate")
            {
                decrease += 2;
            }

            this.Stamina -= decrease;
        }

        public abstract void Rest(int daysCount);

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{this.GetType().Name} - Name: {Name}, Stamina: {Stamina}");
            stringBuilder.Append($"Peaks conquered: ");

            if (this.conqueredPeaks.Count > 0)
            {
                stringBuilder.AppendLine($"{ConqueredPeaks.Count}");
            }
            else
            {
                stringBuilder.Append("no peaks conquered");
            }

            return stringBuilder.ToString();
        }
    }
}

