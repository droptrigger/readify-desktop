using System.ComponentModel.DataAnnotations;

namespace Readify.DTO
{
    public class BanUserDTO
    {
        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Reason { get; set; }
    }
}
