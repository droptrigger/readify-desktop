using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Books
{
    public class AddBookDTO
    {
        [MaxLength(200, ErrorMessage = "Максимальное количество символов: 200")]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(250, ErrorMessage = "Максимальное количество символов: 250")]
        public string? Description { get; set; } = null!;

        [Required]
        public int PageQuantity { get; set; }

        [Required]
        public int IdAuthor { get; set; }

        [Required]
        public IFormFile CoverImageFIle { get; set; } = null!;

        [Required]
        public IFormFile FileBook { get; set; } = null!;

        public int? IdGenre { get; set; } = null;

        public bool IsPrivate { get; set; } = false;
    }
}
