namespace Readify.DTO.Users
{
    public class AuthorDTO
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// Фотография профиля пользователя
        /// </summary>
        public byte[]? AvatarImage { get; set; } = null!;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string? Description { get; set; } = null!;
    }
}
