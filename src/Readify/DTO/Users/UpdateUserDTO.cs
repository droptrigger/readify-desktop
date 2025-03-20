using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Users
{
    public class UpdateUserDTO
    {
        [Key]
        [Required]
        public int UserId {  get; set; }

        [MinLength(5, ErrorMessage = "Минимальное количество символов: 5")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов: 50")]
        public string? Nickname { get; set; } = null!;

        [MaxLength(150)]
        public string? Description { get; set; } = null!;

        public IFormFile? AvatarImage { get; set; } = null!;

        [MinLength(2, ErrorMessage = "Минимальное количество символов: 2")]
        [MaxLength(100, ErrorMessage = "Максимальное количество символов: 100")]
        public string? Name { get; set; } = null!;

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимальное количество символов: 8")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов: 50")]
        public string? Password { get; set; } = null!;
    }
}
