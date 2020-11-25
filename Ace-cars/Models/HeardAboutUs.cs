using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ace_cars.Models
{
    public partial class HeardAboutUs
    {
    
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

     
    }
}
