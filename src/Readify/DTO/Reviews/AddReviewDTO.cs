using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Reviews
{
    public class AddReviewDTO
    {
        [Required]
        public int IdAuthor { get; set; }

        [Required]
        public int IdBook { get; set; }

        [Required]
        [MaxLength(2000, ErrorMessage = "Максимальное количество символов: 2000")]
        public string Comment { get; set; } = null!;

        [Required]
        public byte Rating { get; set; }
    }
}
