using System.ComponentModel.DataAnnotations;

namespace Readify.DTO.Users
{
    /// <summary>
    /// Сущность для обновления access-токена
    /// </summary>
    public class RefreshDTO
    {
        /// <summary>
        /// Refresh-токен
        /// </summary>
        public string? Token { get; set; } = null!;

        /// <summary>
        /// Устройство, с которого отправлен запрос (desktop)
        /// </summary>
        public string? DeviceType { get; set; }
    }
}
