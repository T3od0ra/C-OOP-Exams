using Handball.Core.Contracts;
using Handball.Models;
using Handball.Models.Contracts;
using Handball.Repositories;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPlayer> players;
        private readonly IRepository<ITeam> teams;

        public Controller()
        {
            players = new PlayerRepository();
            teams = new TeamRepository();
        }
        public string LeagueStandings()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("***League Standings***");

            foreach (var team in teams.Models.OrderByDescending(x => x.PointsEarned)
                .ThenByDescending(x => x.OverallRating)
                .ThenBy(x => x.Name))
            {
                stringBuilder.AppendLine(team.ToString());
            }

            return stringBuilder.ToString().Trim();
        }

        public string NewContract(string playerName, string teamName)
        {
            if (players.GetModel(playerName) == null)
            {
                return $"Player with the name {playerName} does not exist in the {nameof(PlayerRepository)}.";
            }
            if (teams.GetModel(teamName) == null)
            {
                return $"Team with the name {teamName} does not exist in the {nameof(TeamRepository)}.";
            }

            IPlayer player = players.GetModel(playerName);
            ITeam team = teams.GetModel(teamName);

            if (player.Team != null)
            {
                return $"Player {playerName} already signed with {player.Team}.";
            }

            player.JoinTeam(teamName);
            team.SignContract(player);

            return $"Player {playerName} signed a contract with {teamName}.";
        }

        public string NewGame(string firstTeamName, string secondTeamName)
        {
            ITeam firstTeam = teams.GetModel(firstTeamName);
            ITeam secondTeam = teams.GetModel(secondTeamName);

            if (firstTeam.OverallRating != secondTeam.OverallRating)
            {
                ITeam winner;
                ITeam loser;

                if (firstTeam.OverallRating > secondTeam.OverallRating)
                {
                    winner = firstTeam;
                    loser = secondTeam;
                }
                else
                {
                    winner = secondTeam;
                    loser = firstTeam;
                }

                winner.Win();
                loser.Lose();
                return $"Team {winner.Name} wins the game over {loser.Name}!";
            }
            else
            {
                firstTeam.Draw();
                secondTeam.Draw();
                return $"The game between {firstTeam.Name} and {secondTeam.Name} ends in a draw!";
            }
        }

        public string NewPlayer(string typeName, string name)
        {
            if (typeName != nameof(Goalkeeper) && typeName != nameof(CenterBack) && typeName != nameof(ForwardWing))
            {
                return $"{typeName} is invalid position for the application.";
            }
            else if (players.Models.Any(x => x.Name == name))
            {
                string existingPlayerName = players.Models.First(x => x.Name == name).GetType().Name;
                return $"{name} is already added to the {nameof(PlayerRepository)} as {existingPlayerName}.";
            }

                IPlayer player;

            if (typeName == nameof(Goalkeeper))
            {
                player = new Goalkeeper(name);
            }
            else if (typeName == nameof(CenterBack))
            {
                player = new CenterBack(name);
            }
            else
            {
                player = new ForwardWing(name);
            }

            players.AddModel(player);
            return $"{name} is filed for the handball league.";
        }

        public string NewTeam(string name)
        {
            if (teams.ExistsModel(name))
            {
                return $"{name} is already added to the {nameof(TeamRepository)}.";
            }

            
            this.teams.AddModel(new Team(name));
            return $"{name} is successfully added to the {nameof(TeamRepository)}.";
        }

        public string PlayerStatistics(string teamName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("***{teamName}***");

            ITeam team = teams.GetModel(teamName);

            foreach (var player in team.Players.OrderByDescending(x => x.Rating).ThenBy(x => x.Name))
            {
                stringBuilder.AppendLine(player.ToString());
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
