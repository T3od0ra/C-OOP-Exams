using NauticalCatchChallenge.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public abstract class Diver : IDiver
    {
        private string name;
        private int oxygenLevel;
        private double competitionPoints;
        private bool hasHealthIssues;
        private List<string> namesOfFishes;

        public Diver(string name, int oxygenLevel)
        {
            Name = name;
            OxygenLevel = oxygenLevel;
            namesOfFishes = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Diver's name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int OxygenLevel
        {
            get => oxygenLevel;
            protected set
            {
                if (value < 0)
                {
                   value = 0;
                }
                oxygenLevel = value;
            }
        }

        public IReadOnlyCollection<string> Catch => namesOfFishes;

        public double CompetitionPoints { get => competitionPoints; private set => competitionPoints = value; }

        public bool HasHealthIssues { get => hasHealthIssues; private set => hasHealthIssues = value; }

        public void Hit(IFish fish)
        {
            this.OxygenLevel -= fish.TimeToCatch;

            namesOfFishes.Add(fish.Name);

            competitionPoints += fish.Points;
        }

        public abstract void Miss(int TimeToCatch);

        public abstract void RenewOxy();


        public void UpdateHealthStatus()
        {
            if (this.HasHealthIssues == false)
            {
                this.HasHealthIssues = true;
            }
        }

        public override string ToString()
        {
            return $"Diver [ Name: {Name}, Oxygen left: {OxygenLevel}, Fish caught: {namesOfFishes.Count}, Points earned: {CompetitionPoints} ]";
        }
    }
}
