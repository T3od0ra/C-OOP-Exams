using Handball.Models;
using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    public class TeamRepository : IRepository<ITeam>
    {
        private readonly List<ITeam> teams;

        public TeamRepository()
        {
            teams = new List<ITeam>();
        }
        public IReadOnlyCollection<ITeam> Models => teams;

        public void AddModel(ITeam model)
        {
            teams.Add(model);
        }

        public bool ExistsModel(string name)
        {
            if (teams.Any(x => x.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ITeam GetModel(string name) => teams.FirstOrDefault(x => x.Name == name);

        public bool RemoveModel(string name)
        {
            var teamToRemove = teams.Find(x => x.Name == name);

            if (teams.Contains(teamToRemove))
            {
                teams.Remove(teamToRemove);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
