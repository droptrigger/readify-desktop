using Readify.DTO.Books;

namespace Readify.DTO.Library
{
    /// <summary>
    /// Класс для представления книги в библиотеке
    /// </summary>
    public class SeeLibraryBookDTO
    {
        /// <summary>
        /// Id книги
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Последняя прочитанная страница
        /// </summary>
        public int LastPage { get; set; }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int PageQuantity { get; set; }

        /// <summary>
        /// Жанр книги
        /// </summary>
        public GenreDTO Genre { get; set; } = null!;

        /// <summary>
        /// Обложка книги
        /// </summary>
        public byte[] ImageBytes { get; set; } = null!;

        public string PageProgressText
        {
            get => $"стр {LastPage}/{PageQuantity}";
        }
    }
}