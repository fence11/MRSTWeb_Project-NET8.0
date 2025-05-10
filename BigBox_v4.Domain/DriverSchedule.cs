using BigBox_v4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBox_v4.Domain
{
    public class DriverSchedule
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int DriverId { get; set; }
        
        [ForeignKey("DriverId")]
        public Drivers? Driver { get; set; }
        
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        
        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        
        [MaxLength(200)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
        
        // shift duration
        [NotMapped]
        public TimeSpan Duration => EndTime - StartTime;
        
        // status type: Scheduled | Completed | Cancelled
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Scheduled";
    }
}


