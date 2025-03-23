using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Users
{
    public class RegistrationDTO
    {
        [MinLength(5, ErrorMessage = "Минимальное количество символов: 5")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов: 50")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Используйте только английские буквы и цифры")]
        [Required]
        public string? Nickname { get; set; }

        [MaxLength(100, ErrorMessage = "Максимальное количество символов: 100")]
        public string? Name { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [MaxLength(150, ErrorMessage = "Максимальное количество символов: 150")]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимальное количество символов: 8")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов: 50")]
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Code {  get; set; }
    }
}
