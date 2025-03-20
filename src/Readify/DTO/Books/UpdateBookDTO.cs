using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Books
{
    public class UpdateBookDTO
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = "Максимальное количество символов: 200")]
        public string Name { get; set; } = null!;

        [MaxLength(250, ErrorMessage = "Максимальное количество символов: 200")]
        public string Description { get; set; } = null!;

        public IFormFile? CoverImageFile { get; set; } = null!;

        public int IdGenre { get; set; } = default;

        public bool IsPrivate { get; set; }
    }
}
