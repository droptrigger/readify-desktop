using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readify.DTO
{
    /// <summary>
    /// Класс, хранящий свойства пользователя
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// True - если присутствует картинка
        /// </summary>
        public bool? HasAvatarImage { get; set; }

        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public IFormFile? AvatarImage { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string? Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string? Password { get; set; } = null!;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public int? IdRole { get; set; }

        /// <summary>
        /// True - если забанен
        /// </summary>
        public bool? IsBanned { get; set; }

        /// <summary>
        /// Дата создания аккаунта пользователя
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Access-токен
        /// </summary>
        public string? AccessToken { get; set; }
    }
}
