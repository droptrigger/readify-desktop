using Readify.DTO.Reviews;
using Readify.DTO.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Readify.DTO.Books
{
    public class BookDTO
    {
        /// <summary>
        /// Id книги
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Описание книги
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Количество страниц книги
        /// </summary>
        public int? PageQuantity { get; set; }

        /// <summary>
        /// Автор книги
        /// </summary>
        public AuthorDTO? Author { get; set; }

        /// <summary>
        /// Изображение обложки
        /// </summary>
        public byte[]? CoverImage { get; set; }

        /// <summary>
        /// Файл с PDF-файлом книги
        /// </summary>
        public byte[]? FileBook { get; set; }

        /// <summary>
        /// Жанр книги
        /// </summary>
        public GenreDTO? Genre { get; set; }

        /// <summary>
        /// True - если приватная
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Дата создания книги
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Количество пользователей, добавивших книгу к себе
        /// </summary>
        public int? CountLibraries { get; set; }

        /// <summary>
        /// Список отзывов на книгу
        /// </summary>
        public List<SeeReviewDTO>? Reviews { get; set; }
    }
}
