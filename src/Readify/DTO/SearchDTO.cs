using Readify.DTO.Books;
using Readify.DTO.Users;

namespace Readify.DTO
{
    public class SearchDTO
    {
        /// <summary>
        /// Поисковой запрос
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Найденные книги
        /// </summary>
        public List<SeeBookDTO>? FoundBooks { get; set; }

        /// <summary>
        /// Найденные пользователи
        /// </summary>
        public List<AuthorDTO>? FoundUsers { get; set; }
    }
}
