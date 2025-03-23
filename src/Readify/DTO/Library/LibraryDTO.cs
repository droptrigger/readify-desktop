using Readify.DTO.Books;
using System;
using System.Collections.Generic;

namespace Readify.DTO.Library
{
    public class LibraryDTO
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int? IdUser { get; set; }

        /// <summary>
        /// Книга
        /// </summary>
        public SeeBookDTO? Book { get; set; }

        /// <summary>
        /// Дата добавления книги
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Страница, на которой было остановлено чтение
        /// </summary>
        public int ProgressPage { get; set; }

        /// <summary>
        /// Закладки книги
        /// </summary>
        public List<BookmarkDTO>? Bookmarks { get; set; }
    }
}
