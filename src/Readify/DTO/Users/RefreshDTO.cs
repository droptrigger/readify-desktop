using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Users
{
    public class RefreshDTO
    {
        public string? refreshToken { get; set; } = null!;

        [Required]
        public string? Device { get; set; }
    }
}
