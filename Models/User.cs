using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarService.Models;

namespace CarService.Models
{
    public class User : IdentityUser
    {
        [Required] [StringLength(100)] public string Name { get; set; }

        [Required] [StringLength(100)] public string Surname { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<RepairRecord> RepairRecords { get; set; }
        public ICollection<Master> Masters { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}