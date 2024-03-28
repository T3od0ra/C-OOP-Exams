﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Models
{
    public class Goalkeeper : Player
    {
        private const double initialRating = 2.5;
        private const double increaseRating = 0.75;
        private const double decreaseRating = 1.25;
        public Goalkeeper(string name) : base(name, initialRating)
        {
        }

        public override void DecreaseRating()
        {
            base.Rating -= decreaseRating;
            if(base.Rating < 1)
            { 
                base.Rating = 1;
            }
        }

        public override void IncreaseRating()
        {
            base.Rating += increaseRating;
            if (base.Rating > 10)
            {
                base.Rating = 10;
            }
        }
    }
}
