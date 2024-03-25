using NauticalCatchChallenge.Core.Contracts;
using NauticalCatchChallenge.Models;
using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories;
using NauticalCatchChallenge.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IDiver> divers;
        private readonly IRepository<IFish> fishes;

        public Controller()
        {
            divers = new DiverRepository();
            fishes = new FishRepository();
        }
        public string ChaseFish(string diverName, string fishName, bool isLucky)
        {
            if (divers.GetModel(diverName) == null)
            {
                return $"{nameof(DiverRepository)} has no {diverName} registered for the competition.";
            }
            if (fishes.GetModel(fishName) == null)
            {
                return $"{fishName} is not allowed to be caught in this competition.";
            }

            IDiver diver = divers.GetModel(diverName);

            if (diver.HasHealthIssues)
            {
                return $"{diverName} will not be allowed to dive, due to health issues.";
            }

            IFish fish = fishes.GetModel(fishName);

            if (diver.OxygenLevel < fish.TimeToCatch)
            {
                diver.Miss(fish.TimeToCatch);

                if (diver.OxygenLevel == 0)
                {
                    diver.UpdateHealthStatus();
                }
                return $"{diverName} missed a good {fishName}.";
            }
            else if (diver.OxygenLevel == fish.TimeToCatch && !isLucky)
            {
                diver.Miss(fish.TimeToCatch);

                if (diver.OxygenLevel == 0)
                {
                    diver.UpdateHealthStatus();
                }

                return $"{diverName} missed a good {fishName}.";
            }

            else
            {
                diver.Hit(fish);

                if (diver.OxygenLevel == 0)
                {
                    diver.UpdateHealthStatus();
                }
                return $"{diverName} hits a {fish.Points}pt. {fishName}.";
            }
        }

        public string CompetitionStatistics()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("**Nautical-Catch-Challenge**");

            foreach (var diver in divers.Models.Where(x => x.HasHealthIssues == false)
                .OrderByDescending(x => x.CompetitionPoints)
                .ThenByDescending(x => x.Catch.Count)
                .ThenBy(x => x.Name))
            {
                stringBuilder.AppendLine(diver.ToString());
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public string DiveIntoCompetition(string diverType, string diverName)
        {

            if (diverType != nameof(FreeDiver) && diverType != nameof(ScubaDiver))
            {
                return $"{diverType} is not allowed in our competition.";
            }

            else if (divers.Models.Any(x => x.Name == diverName))
            {
                return ($"{diverName} is already a participant -> {nameof(DiverRepository)}.");
            }

            else
            {
                IDiver diver;

                if (diverType == nameof(FreeDiver))
                {
                    diver = new FreeDiver(diverName);
                }

                else
                { 
                    diver = new ScubaDiver(diverName);
                }

                divers.AddModel(diver);
                return $"{diverName} is successfully registered for the competition -> {nameof(DiverRepository)}.";
            }
        }

        public string DiverCatchReport(string diverName)
        {
            IDiver diver = divers.GetModel(diverName);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(divers.ToString());
            stringBuilder.AppendLine("Catch Report:");

            foreach (var fishName in diver.Catch)
            {
                IFish fish = fishes.GetModel(fishName);
                stringBuilder.AppendLine(fish.ToString());
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public string HealthRecovery()
        {
           int counter  = 0;

            foreach (var diver in divers.Models.Where(x => x.HasHealthIssues == true))
            {
                counter++;
                diver.UpdateHealthStatus();
                diver.RenewOxy();
            }

            return $"Divers recovered: {counter}";
        }

        public string SwimIntoCompetition(string fishType, string fishName, double points)
        {
            if (fishType != nameof(ReefFish) && fishType != nameof(DeepSeaFish) && fishType != nameof(PredatoryFish))
            {
               return $"{fishType} is forbidden for chasing in our competition.";
            }
            else if (fishes.Models.Any(x => x.Name == fishName))
            {
                return $"{fishName} is already a participant -> {nameof(FishRepository)}.";
            }

            else
            {
                IFish fish;

                if (fishType == nameof(ReefFish))
                {
                    fish = new ReefFish(fishName, points);
                }

                else if (fishType == nameof(DeepSeaFish))
                {
                    fish = new DeepSeaFish(fishName, points);
                }
                else
                {
                    fish = new PredatoryFish(fishName, points);
                }

                fishes.AddModel(fish);
            }

            return $"{fishName} is allowed for chasing.";
        }
    }
}
