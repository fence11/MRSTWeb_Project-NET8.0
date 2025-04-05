using System.ComponentModel.DataAnnotations;

namespace BigBox_v4.Domain
{
    public class BusinessModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public BusinessModel()
        {
            CreatedDate = DateTime.Now;
        }
    }
}

