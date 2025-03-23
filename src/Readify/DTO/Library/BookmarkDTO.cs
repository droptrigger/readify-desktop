using System;

namespace Readify.DTO.Library
{
    public class BookmarkDTO
    {
        /// <summary>
        /// Id закладки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id книги пользователя
        /// </summary>
        public int? IdLibrary { get; set; }

        /// <summary>
        /// Страница, на которой закладка
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Текст закладки
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Дата создания закладки
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
