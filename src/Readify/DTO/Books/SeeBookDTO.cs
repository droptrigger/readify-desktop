using Readify.DTO.Users;

namespace Readify.DTO.Books
{
    /// <summary>
    /// Класс для представления книги в профиле пользователя
    /// </summary>
    public class SeeBookDTO
    {
        /// <summary>
        /// Id книги
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Изображение обложки книги
        /// </summary>
        public byte[]? CoverImage { get; set; }
    
        /// <summary>
        /// Автор книги
        /// </summary>
        public AuthorDTO? Author { get; set; }

        /// <summary>
        /// Жанр книги
        /// </summary>
        public GenreDTO? Genre { get; set; }
    }
}
