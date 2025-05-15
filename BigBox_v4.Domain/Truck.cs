using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBox_v4.Domain
{
    public class Truck
    {
        [Key]
        public int TruckID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Truck Brand")]
        public required string TruckBrand { get; set; }

        [Display(Name = "Tow Width")]
        public decimal? TowWidth { get; set; }

        [Display(Name = "Tow Height")]
        public decimal? TowHeight { get; set; }

        [Display(Name = "Tow Length")]
        public decimal? TowLength { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Truck State")]
        public string TruckState { get; set; } = "Idle"; 

        [MaxLength(20)]
        [Display(Name = "License Plate")]
        public string? LicensePlate { get; set; }

        [Display(Name = "Manufacture Year")]
        public int? ManufactureYear { get; set; }

        [Display(Name = "Max Load Capacity")]
        public decimal? MaxLoadCapacity { get; set; }

        [MaxLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}