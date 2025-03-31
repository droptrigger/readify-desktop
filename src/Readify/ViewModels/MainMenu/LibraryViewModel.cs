using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.Services.Base;
using Readify.ViewModels.Base;

namespace Readify.ViewModels.MainMenu
{
    public class LibraryViewModel : BaseViewModel
    {
        private ILibraryService _libraryService;

        private SeeLibrariesDTO _library = null!;

        private SeeLibraryBookDTO? _lastBook = null;
        private List<GenreDTO>? _genres = null;
        private List<SeeLibraryBookDTO>? _notFullyReadBooks = null;
        private List<SeeLibraryBookDTO>? _fullyReadBooks = null;
        private List<SeeLibraryBookDTO>? _deployedBooks = null;

        /// <summary>
        /// Библиотека пользователя
        /// </summary>
        public SeeLibrariesDTO Library
        {
            get => _library;
            set => SetField(ref _library, value);
        }

        /// <summary>
        /// Видимость надписи "Посмотреть все" у непрочитанных книг
        /// </summary>
        public bool IsShowAllNotFullyReadBooksVisible
        {
            get => NotFullyReadBooks?.Count > 5;
        }

        /// <summary>
        /// Видимость надписи "Посмотреть все" у выложенных книг
        /// </summary>
        public bool IsShowAllDeployedBooksVisible
        {
            get => DeployedBooks?.Count > 5;
        }

        /// <summary>
        /// Видимость надписи "Посмотреть все" у прочитанных книг
        /// </summary>
        public bool IsShowAllFullyReadBooksVisible
        {
            get => FullyReadBooks?.Count > 5;
        }

        public bool IsLastBookVisible
        {
            get => _lastBook != null;
        }

        /// <summary>
        /// Последняя книга, которую читал пользователь
        /// </summary>
        public SeeLibraryBookDTO LastBook
        {
            get => _lastBook!;
            set => SetField(ref _lastBook, value);
        }

        /// <summary>
        /// Жанры
        /// </summary>
        public List<GenreDTO> Genres
        {
            get => _genres!;
            set => SetField(ref _genres, value);
        }

        /// <summary>
        /// Не прочитанные книги пользователя
        /// </summary>
        public List<SeeLibraryBookDTO> NotFullyReadBooks
        {
            get => _notFullyReadBooks!;
            set => SetField(ref _notFullyReadBooks, value);
        }

        /// <summary>
        /// Полностью прочитанные книги пользователя
        /// </summary>
        public List<SeeLibraryBookDTO> FullyReadBooks
        {
            get => _fullyReadBooks!;
            set => SetField(ref _fullyReadBooks, value);
        }

        /// <summary>
        /// Загруженные пользователем книги
        /// </summary>
        public List<SeeLibraryBookDTO> DeployedBooks
        {
            get => _deployedBooks!;
            set => SetField(ref _deployedBooks, value);
        }

        public string LastBookPageText
        {
            get => $"стр {LastBook?.LastPage}/{LastBook?.PageQuantity}";
        }

        private void SetValues(SeeLibrariesDTO library)
        {
            Genres = Library.Genres!;
            LastBook = Library.LastBook!;
            FullyReadBooks = Library.FullyReadBooks!;
            NotFullyReadBooks = Library.NotFullyReadBooks!;
            DeployedBooks = Library.DeployedBooks!;
        }

        public LibraryViewModel(ILibraryService libraryService)
        {
            _libraryService = libraryService;
            Library = App.CurrentUserLibrary;
            SetValues(Library);
        }
    }
}
