using HighwayToPeak.Core.Contracts;
using HighwayToPeak.Models;
using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Repositories;
using HighwayToPeak.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPeak> peaks;
        private readonly IRepository<IClimber> climbers;
        private readonly IBaseCamp baseCamp;

        public Controller()
        {
            peaks = new PeakRepository();
            climbers = new ClimberRepository();
            baseCamp = new BaseCamp();
        }
        
        public string AddPeak(string name, int elevation, string difficultyLevel)
        {
            if (peaks.Get(name) != null)
            {
                return $"{name} is already added as a valid mountain destination.";
            }

            if (difficultyLevel != "Hard" && difficultyLevel != "Extreme" && difficultyLevel != "Moderate")
            {
                return $"{difficultyLevel} peaks are not allowed for international climbers.";
            }

            IPeak peak = new Peak(name, elevation, difficultyLevel);
            peaks.Add(peak);

            return $"{name} is allowed for international climbing. See details in {nameof(PeakRepository)}.";
        }

        public string AttackPeak(string climberName, string peakName)
        {
            if (climbers.Get(climberName) == null)
            {
                return $"Climber - {climberName}, has not arrived at the BaseCamp yet.";
            }

            if (peaks.Get(peakName) == null)
            {
                return $"{peakName} is not allowed for international climbing.";
            }

           if (!baseCamp.Residents.Contains(climberName))
            {
                return $"{climberName} not found for gearing and instructions. The attack of {peakName} will be postponed.";
            }

            IClimber climber = climbers.Get(climberName);
            IPeak peak = peaks.Get(peakName);

            if (peak.DifficultyLevel == "Extreem" && climber.GetType().Name == nameof(NaturalClimber))
            {
                return $"{climberName} does not cover the requirements for climbing {peakName}.";
            }

            baseCamp.LeaveCamp(climberName);
            climber.Climb(peak);

            if (climber.Stamina == 0)
            {
                return $"{climberName} did not return to BaseCamp.";
            }
           
            baseCamp.ArriveAtCamp(climberName);
            return $"{climberName} successfully conquered {peakName} and returned to BaseCamp.";
        }
        
        public string BaseCampReport()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (baseCamp.Residents.Count == 0)
            {
                return "BaseCamp is currently empty.";
            }

            stringBuilder.AppendLine("BaseCamp residents:");

            foreach (var climberName in baseCamp.Residents)
            {
                IClimber climber = climbers.Get(climberName);
                stringBuilder.AppendLine($"Name: {climber.Name}, Stamina: {climber.Stamina}, Count of Conquered Peaks: {climber.ConqueredPeaks.Count}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public string CampRecovery(string climberName, int daysToRecover)
        {
            if (!baseCamp.Residents.Contains(climberName))
            {
                return $"{climberName} not found at the BaseCamp.";
            }

            IClimber climber = climbers.Get(climberName);

            if (climber.Stamina == 10)
            {
                return $"{climberName} has no need of recovery.";
            }
            
             climber.Rest(daysToRecover);
             return $"{climberName} has been recovering for {daysToRecover} days and is ready to attack the mountain.";
        }

        public string NewClimberAtCamp(string name, bool isOxygenUsed)
        {
            if (climbers.Get(name) != null)
            {
                return $"{name} is a participant in {nameof(ClimberRepository)} and cannot be duplicated.";
            } 

            IClimber climber;

            if (isOxygenUsed)
            {
                climber = new OxygenClimber(name);
            }
            else
            {
                climber = new NaturalClimber(name);
            }

            climbers.Add(climber);
            baseCamp.ArriveAtCamp(name);

            return $"{name} has arrived at the BaseCamp and will wait for the best conditions.";

        }

        public string OverallStatistics()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("***Highway-To-Peak***");

            foreach (var climber in climbers.All
                .OrderByDescending(x => x.ConqueredPeaks.Count)
                .ThenBy(x => x.Name))
            {
                stringBuilder.AppendLine(climber.ToString().TrimEnd());
                var orderedPeaks = new List<IPeak>();

                foreach (var peak in climber.ConqueredPeaks)
                {
                    IPeak order = peaks.Get(peak);
                    orderedPeaks.Add(order);
                }

                foreach (var peak in orderedPeaks.OrderByDescending(x => x.Elevation))
                {
                    stringBuilder.AppendLine(peak.ToString());
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
