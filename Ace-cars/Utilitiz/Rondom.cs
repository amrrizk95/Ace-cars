using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace_cars
{
    public class RandomNumbers : Random
    {
        public RandomNumbers() :base() { }

        public  double NextDouble(double minimum, double maximum)
        {
            return base.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
