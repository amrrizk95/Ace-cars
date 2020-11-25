using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ace_cars.Models
{
    public partial class Car
    {


        public string Name { get; set; }
        [Key]
        public int CarId { get; set; }

    }
}
