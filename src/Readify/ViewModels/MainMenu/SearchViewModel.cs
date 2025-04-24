using CommunityToolkit.Mvvm.Input;
using Readify.DTO;
using Readify.DTO.Books;
using Readify.DTO.Users;
using Readify.Pages.MainMenu;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Readify.ViewModels.MainMenu
{
    public class SearchViewModel : BaseViewModel
    {
        private SearchDTO _serachDTO = null!;
        private IBookService _bookService;
        private ILibraryService _libraryService;
        private IUserService _userService;


        private List<SeeBookDTO> _foundBooks;
        private string _selectedGenre;

        public string SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
                Filter();
            }
        }


        /// <summary>
        /// Поисковые данные
        /// </summary>
        public SearchDTO Search
        {
            get => _serachDTO;
            set => SetField(ref _serachDTO, value);
        }

        /// <summary>
        /// Поисковой запрос
        /// </summary>
        public string SearchText
        {
            get => Search.SearchText;
        }

        /// <summary>
        /// Список найденных книг
        /// </summary>
        public List<SeeBookDTO>? FoundBooks
        {
            get => _foundBooks;
            set => SetField(ref _foundBooks, value);
        }

        public List<string> Genres
        {
            get => FoundBooks?
                .Where(b => b?.Genre?.Name != null)
                .Select(b => b.Genre.Name)
                .Distinct()
                .ToList()
                ?? new List<string>();
        }

        /// <summary>
        /// Список найденных пользователей
        /// </summary>
        public List<AuthorDTO>? FoundUsers
        {
            get => Search.FoundUsers;
        }

        /// <summary>
        /// Видимость блока "Ничего не найдено" у пользователей
        /// </summary>
        public bool IsBlockNotUserVisible
        {
            get => FoundUsers?.Count == 0;
        }

        /// <summary>
        /// Видимость области прокрутки пользователей
        /// </summary>
        public bool IsSearchUserVisible
        {
            get => !IsBlockNotUserVisible;
        }

        /// <summary>
        /// Видимость блока "Ничего не найдено" у книг
        /// </summary>
        public bool IsBlockNotBookVisible
        {
            get => FoundBooks?.Count == 0;
        }

        /// <summary>
        /// Видимость области прокрутки книг
        /// </summary>
        public bool IsSearchBookVisible
        {
            get => !IsBlockNotBookVisible;
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на книгу
        /// </summary>
        public ICommand ShowBookCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на книгу
        /// </summary>
        public ICommand ShowUserCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serachDTO"></param>
        public SearchViewModel(
            SearchDTO serachDTO, 
            IBookService bookService,
            ILibraryService libraryService,
            IUserService userService
            )
        {
            Search = serachDTO;
            _bookService = bookService;
            _libraryService = libraryService;
            _userService = userService;
            FoundBooks = Search.FoundBooks;

            ShowBookCommand = new AsyncRelayCommand<int>(ExecuteShowBookAsync);
            ShowUserCommand = new AsyncRelayCommand<int>(ExecuteShowUserAsync);
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

        public async Task ExecuteShowUserAsync(int idUser)
        {
            var getUser = await _userService.GetUserByIdAsync(idUser);

            if (getUser == null)
                MessageBox.Show("Ошибка");

            ProfilePage page = new ProfilePage(getUser);
            App.MainMenuPage.MainMenuFrame.Navigate(page);
        }

        private void Filter()
        {
            if (Search.FoundBooks == null) return;

            var filtered = Search.FoundBooks
                .Where(b =>
                    (string.IsNullOrEmpty(SelectedGenre) || b.Genre?.Name == SelectedGenre)
                )
                .ToList();

            FoundBooks = filtered;
        }
    }
}
