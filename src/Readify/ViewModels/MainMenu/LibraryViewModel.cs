using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.Pages.MainMenu;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.MainMenu
{
    public class LibraryViewModel : BaseViewModel
    {
        private ILibraryService _libraryService;
        private IBookService _bookService;

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
        /// Видимость последней прочитанной книги
        /// </summary>
        public bool IsLastBookVisible
        {
            get => LastBook is not null;
        }

        /// <summary>
        /// Видимость блока с недочитанными книгами
        /// </summary>
        public bool IsNotReadVisible
        {
            get => Library.NotFullyReadBooks?.Count > 0;
        }

        /// <summary>
        /// Видимость заглушки блока с недочитанными книгами
        /// </summary>
        public bool IsBlockNotReadVisible
        {
            get => !IsNotReadVisible;
        }

        /// <summary>
        /// Видимость блока с опубликованными книгами
        /// </summary>
        public bool IsDeployedVisible
        {
            get => Library.DeployedBooks?.Count > 0;
        }


        /// <summary>
        /// Видимость заглушки блока с опубликованными книгами
        /// </summary>
        public bool IsBlockDeployedVisible
        {
            get => !IsDeployedVisible;
        }

        /// <summary>
        /// Видимость блока с прочитанными книгами
        /// </summary>
        public bool IsReadVisible
        {
            get => Library.FullyReadBooks?.Count > 0;
        }

        /// <summary>
        /// Видимость заглушки блока с прочитанными книгами
        /// </summary>
        public bool IsBlockReadVisible
        {
            get => !IsReadVisible;
        }

        /// <summary>
        /// Видимость последней прочитанной книги
        /// </summary>
        public bool IsBlockLastBookVisible
        {
            get => !IsLastBookVisible;
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

        /// <summary>
        /// Последняя книга, которую читал пользователь
        /// </summary>
        public SeeLibraryBookDTO LastBook
        {
            get => _lastBook;
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

        /// <summary>
        /// Команда, срабатывающая при нажатии на книгу
        /// </summary>
        public ICommand ShowBookCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="libraryService"></param>
        public LibraryViewModel(
            ILibraryService libraryService, 
            IBookService bookService)
        {
            _libraryService = libraryService;
            Library = App.CurrentUserLibrary;
            SetValues(Library);
            ShowBookCommand = new AsyncRelayCommand<int>(ExecuteShowBookAsync);
            _bookService = bookService;
        }

        private async Task ExecuteShowBookAsync(int idBook)
        {
            if (idBook > 0)
            {
                App.CurrentUserLibrary = await _libraryService.GetBooksByUserIdAsync(App.CurrentUser.Id);
                var getBook = await _bookService.GetBookByIdAsync(idBook);

                if (getBook == null)
                    MessageBox.Show("Ошибка");

                BookPage page = new BookPage(getBook!);

                App.MainMenuPage?.MainMenuFrame.Navigate(page);
            }
        }
    }
}
