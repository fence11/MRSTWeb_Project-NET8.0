using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public class BoxSize
    {
        [Key]
        public int BoxSizeId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Width (cm)")]
        public decimal Width { get; set; }

        [Required]
        [Display(Name = "Height (cm)")]
        public decimal Height { get; set; }

        [Required]
        [Display(Name = "Length (cm)")]
        public decimal Length { get; set; }

        [Display(Name = "Description")]
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
