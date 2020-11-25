using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ace_cars.Models
{
    public partial class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? VisitDate { get; set; }
        public int? CarId { get; set; }
        public int? HeardAboutUsId { get; set; }

        public virtual Car Car { get; set; }
        public virtual HeardAboutUs HeardAboutUs { get; set; }
    }
}
