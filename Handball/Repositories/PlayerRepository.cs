using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private readonly List<IPlayer> players;

        public PlayerRepository()
        {
            players = new List<IPlayer>();
        } 
        public IReadOnlyCollection<IPlayer> Models => players;

        public void AddModel(IPlayer model)
        {
            players.Add(model);
        }

        public bool ExistsModel(string name)
        {
            if (players.Any(x => x.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IPlayer GetModel(string name) => players.FirstOrDefault(x => x.Name == name);

        public bool RemoveModel(string name)
        {
           var playerToRemove = players.Find(x => x.Name == name);

            if (players.Contains(playerToRemove))
            {
                players.Remove(playerToRemove);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
