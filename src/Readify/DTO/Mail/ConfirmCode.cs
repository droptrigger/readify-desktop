using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Mail
{
    public class ConfirmCode
    {
        [DataType(DataType.EmailAddress)]
        [MaxLength(150, ErrorMessage = "Максимальное количество символов: 150")]
        [Required]
        public string? Email { get; set; }

        [MaxLength(6, ErrorMessage = "Максимальное количество символов: 6")]
        [Required]
        public string? Code { get; set; }
    }
}
