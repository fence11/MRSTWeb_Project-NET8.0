using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public class Box
    {
        [Key]
        public int BoxId { get; set; }

        [Required]
        [Display(Name = "Width (cm)")]
        public decimal Width { get; set; }

        [Required]
        [Display(Name = "Height (cm)")]
        public decimal Height { get; set; }

        [Required]
        [Display(Name = "Length (cm)")]
        public decimal Length { get; set; }

        [Required]
        [Display(Name = "Weight (kg)")]
        public decimal Weight { get; set; }

        [Display(Name = "This Way Up")]
        public bool ThisWayUp { get; set; }

        [Display(Name = "Fragile")]
        public bool Fragile { get; set; }

        [Display(Name = "Handle With Care")]
        public bool HandleWithCare { get; set; }

        [Display(Name = "Keep Dry")]
        public bool KeepDry { get; set; }

        [Display(Name = "Keep Upright")]
        public bool KeepUpright { get; set; }

        [Display(Name = "Perishable")]
        public bool Perishable { get; set; }

        [Display(Name = "Do Not Stack")]
        public bool DoNotStack { get; set; }

        [Display(Name = "Flammable")]
        public bool Flammable { get; set; }

        [Display(Name = "Explosive")]
        public bool Explosive { get; set; }

        [Display(Name = "Do Not Open Before Date")]
        public DateTime? DoNotOpenBeforeDate { get; set; }

        [Display(Name = "Additional Notes")]
        [MaxLength(500)]
        public string? AdditionalNotes { get; set; }

        // Navigation properties
        public int? DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual Models.Drivers? Driver { get; set; }

        public int? TruckId { get; set; }
        [ForeignKey("TruckId")]
        public virtual Truck? Truck { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        [MaxLength(20)]
        public string Status { get; set; } = "New";
    }
}
