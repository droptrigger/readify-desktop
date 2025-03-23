using Readify.DTO.Users;

namespace Readify.DTO
{
    /// <summary>
    /// Сущность, отвечающая за хранение ответа серевера с токенами доступа
    /// </summary>
    public class LoginResponce
    {
        /// <summary>
        /// Access-токен
        /// </summary>
        public string? Token { get; set; }
        
        /// <summary>
        /// Refresh-токен
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Данные пользователя
        /// </summary>
        public UserDTO? User { get; set; }
    }
}
