using Readify.DTO.Library;
using Readify.DTO.Reviews;

namespace Readify.DTO.Users
{
    public class UserDTO
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Фотография профиля пользователя
        /// </summary>
        public byte[]? AvatarImage { get; set; } = null!;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string? Email { get; set; } = null!;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public int? IdRole { get; set; }

        /// <summary>
        /// Дата создания аккаунта
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Список книг пользователя
        /// </summary>
        public List<LibraryDTO>? Books { get; set; }

        /// <summary>
        /// Список подписчиков пользователя
        /// </summary>
        public List<AuthorDTO>? Subscribers { get; set; }

        /// <summary>
        /// Список подписок пользователя
        /// </summary>
        public List<AuthorDTO>? Subscriptions { get; set; }

        /// <summary>
        /// Список отзывов пользователя
        /// </summary>
        public List<SeeReviewDTO>? Reviews { get; set; }
    }
}
