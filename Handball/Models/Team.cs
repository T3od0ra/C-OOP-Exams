using Handball.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Models
{
    public class Team : ITeam
    {
        private string name;
        private int pointsEarned;
        private double overallRating;
        private List<IPlayer> players;

        public Team(string name)
        {
            Name = name;
            players = new List<IPlayer>();
            pointsEarned = 0;
        }
        public string Name
        {
            get => name;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Team name cannot be null or empty.");
                }
                name = value;
            }
        }
        public int PointsEarned
        {
            get => pointsEarned;
            private set
            {
                pointsEarned = value = 0;
            }
        }
        public double OverallRating => players.Count == 0 ? 0 : Math.Round(players.Average(player => player.Rating), 2);
        public IReadOnlyCollection<IPlayer> Players => this.players.AsReadOnly();

        public void Draw()
        {
            pointsEarned += 1;
            this.Players.FirstOrDefault(x => x.GetType().Name == nameof(Goalkeeper)).IncreaseRating();
        }

        public void Lose()
        {
            foreach (var player in players)
            {
                player.DecreaseRating();
            }
        }

        public void SignContract(IPlayer player)
        {
           this.players.Add(player);
        }

        public void Win()
        {
            pointsEarned += 3;

            foreach (var player in players)
            {
                player.IncreaseRating();
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Team: {this.Name} Points: {this.PointsEarned}");
            stringBuilder.AppendLine($"--Overall rating: {OverallRating}");
            stringBuilder.Append("--Players: ");

            if (this.Players.Any())
            {
                var names = this.Players.Select(x => x.Name);
                stringBuilder.Append(string.Join(", ", names));
            }
            else
            {
                stringBuilder.Append("none");
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
