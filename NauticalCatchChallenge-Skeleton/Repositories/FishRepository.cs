using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Repositories
{
    public class FishRepository : IRepository<IFish>
    {
        private readonly List<IFish> fishes;

        public FishRepository()
        {
            fishes = new List<IFish>();
        }
        public IReadOnlyCollection<IFish> Models => fishes;

        public void AddModel(IFish model)
        {
            fishes.Add(model);
        }

        public IFish GetModel(string name) => Models.FirstOrDefault(x => x.Name == name);
}
}

