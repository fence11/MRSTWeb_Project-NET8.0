using System;
using System.ComponentModel.DataAnnotations;

namespace BigBox_v4.Models
{
    public class Drivers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LicenseNumber { get; set; }

        [Required]
        public DateTime EmploymentDate { get; set; }

        //public Drivers()
        //{
        //    EmploymentDate = DateTime.Now;
        //}
    }
}

