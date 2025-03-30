using Readify.DTO.Books;

namespace Readify.DTO.Library
{
    /// <summary>
    /// Класс для представления библиотеки
    /// </summary>
    public class SeeLibrariesDTO
    {
        /// <summary>
        /// Последняя книга, которую читал пользователь
        /// </summary>
        public SeeLibraryBookDTO? LastBook { get; set; }

        /// <summary>
        /// Жанры всех книг
        /// </summary>
        public List<GenreDTO>? Genres { get; set; }

        /// <summary>
        /// Список не дочитанных книг
        /// </summary>
        public List<SeeLibraryBookDTO>? NotFullyReadBooks { get; set; }

        /// <summary>
        /// Список книг, которых опуликовал автор
        /// </summary>
        public List<SeeLibraryBookDTO>? DeployedBooks { get; set; }

        /// <summary>
        /// Список дочитанных книг
        /// </summary>
        public List<SeeLibraryBookDTO>? FullyReadBooks { get; set; }
    }
}
