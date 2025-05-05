using CommunityToolkit.Mvvm.Input;
using netoaster;
using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.DTO.Reviews;
using Readify.Pages.MainMenu;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Printing;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private ILibraryService _libraryService;
        private IBookService _bookService;
        private IUserService _userService;

        private string _addRatingText;
        private byte _selectedRating;

        private List<byte> _ratings = new List<byte>()
        {
            1, 2, 3, 4, 5
        };

        private BookDTO _book = null!;
        private bool _addLibraryVisible;
        private bool _deleteLibraryVisible;

        private bool _ratingBlockVisible;

        public string AddRatingText
        {
            get => _addRatingText;
            set => SetField(ref _addRatingText, value);
        }

        public List<byte> Ratings
        {
            get => _ratings;
        } 

        public byte SelectedRating
        {
            get => _selectedRating;
            set => SetField(ref  _selectedRating, value);
        }

        /// <summary>
        /// Текущая книга
        /// </summary>
        public BookDTO Book
        {
            get => _book;
            set => SetField(ref _book, value); 
        }

        /// <summary>
        /// Последняя прочитанная книга пользователя
        /// </summary>
        public SeeLibraryBookDTO? LastBook
        {
            get => App.CurrentUserLibrary.LastBook;
        } 

        /// <summary>
        /// Видимость последней прочитанной книги
        /// </summary>
        public bool IsLastBookVisible
        {
            get => LastBook is not null;
        }

        /// <summary>
        /// Видимость кнопки "Удалить из библиотеки"
        /// </summary>
        public bool IsBtnDeleteFromLibraryVisible
        {
            get => _deleteLibraryVisible;
            set => SetField(ref _deleteLibraryVisible, value);
        }

        /// <summary>
        /// Видимость кнопки "Добавить в библиотеки"
        /// </summary>
        public bool IsBtnAddToLibraryVisible
        {
            get => _addLibraryVisible;
            set => SetField(ref _addLibraryVisible, value);
        }

        /// <summary>
        /// Видимость кнопки "Написать отзыв"
        /// </summary>
        public bool IsBtnWriteReviewVisible
        {
            get => _ratingBlockVisible;
            set => SetField(ref _ratingBlockVisible, value);
        }

        /// <summary>
        /// Видимость кнопки "Удалить с сервиса"
        /// </summary>
        public bool IsBtnDeleteFromServiceVisible
        {
            get 
            {
                return App.CurrentUser.IdRole == 2 || App.CurrentUser.Id == Book.Author!.Id;
            }
        }

        /// <summary>
        /// Текст кто опубликовал книгу
        /// </summary>
        public string DeployerText
        {
            get => $"Опубликовал: {Book.Author!.Nickname}";
        }

        /// <summary>
        /// Текст с жанром
        /// </summary>
        public string GenreText
        {
            get => $"Жанр: {Book.Genre!.Name}";
        }

        public string RatingText
        {
            get => $"{GetRatingBook(Book)}/5";
        }

        /// <summary>
        /// Команда, срабатывающая, на нажатие кноки "Добавить в библиотеку"
        /// </summary>
        public ICommand AddToLibraryCommand { get; }

        /// <summary>
        /// Команда, срабатывающая, на нажатие кноки "Удалить из библиотки"
        /// </summary>
        public ICommand DeleteFromLibraryCommand { get; }

        /// <summary>
        /// Команда, срабатывающая, на нажатие кноки "Удалить с сервиса"
        /// </summary>
        public ICommand DeleteFromServiceCommand { get; }

        /// <summary>
        /// Команда, срабатывающая, на нажатие кноки "Опубликовать отзыв"
        /// </summary>
        public ICommand AddReviewCommand { get; }

        /// <summary>
        /// Команда, срабатывающая, на нажатие кноки "Читать"
        /// </summary>
        public ICommand ReadBookCommand { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="book"></param>
        public BookViewModel(BookDTO book, 
            ILibraryService libraryService, 
            IBookService bookService,
            IUserService userService)
        {
            Book = book;
            SetLibraryButtonsVisible();
            _libraryService = libraryService;
            _bookService = bookService;
            _userService = userService;

            AddToLibraryCommand = new AsyncRelayCommand(ExecuteAddToLibraryAsync);
            DeleteFromLibraryCommand = new AsyncRelayCommand(ExecuteDeleteFromLibraryAsync);
            DeleteFromServiceCommand = new AsyncRelayCommand(ExecuteDeleteFromServiceAsync);
            AddReviewCommand = new AsyncRelayCommand(ExecuteAddReviewAsync);
        }


        private async Task ExecuteAddReviewAsync()
        {
            AddReviewDTO reviewDTO = new AddReviewDTO
            {
                IdAuthor = App.CurrentUser.Id,
                IdBook = (int)Book.Id!
            };
            if (AddRatingText is null)
            {
                WarningToaster.Toast(App.Current.MainWindow,
                    title: "Ошибка!",
                    message: $"Введите текст отзыва",
                    position: ToasterPosition.ApplicationBottomRight,
                    animation: ToasterAnimation.SlideInFromRight,
                    margin: 0
                    );
                return;
            }

            if (AddRatingText.Length > 2000)
            {
                WarningToaster.Toast(App.Current.MainWindow,
                    title: "Ошибка!",
                    message: $"Максимальное количество символов в отзыве - 2000",
                    position: ToasterPosition.ApplicationBottomRight,
                    animation: ToasterAnimation.SlideInFromRight,
                    margin: 0
                    );
                return;
            }

            reviewDTO.Comment = AddRatingText;

            if (SelectedRating <= 0 || SelectedRating > 5)
            {
                WarningToaster.Toast(App.Current.MainWindow,
                    title: "Ошибка!",
                    message: $"Выберите рейтинг",
                    position: ToasterPosition.ApplicationBottomRight,
                    animation: ToasterAnimation.SlideInFromRight,
                    margin: 0
                    );
                return;
            }

            reviewDTO.Rating = SelectedRating;

            try
            {
                if (await _bookService.AddReviewToBook(reviewDTO))
                {
                    Book = await _bookService.GetBookByIdAsync((int)Book.Id!);
                    SuccessToaster.Toast(App.Current.MainWindow,
                        title: "Успешно!",
                        message: "Вы успешно опубликовали отзыв!",
                        position: ToasterPosition.ApplicationBottomRight,
                        animation: ToasterAnimation.SlideInFromRight,
                        margin: 0
                        );
                    IsBtnWriteReviewVisible = false;
                }
            }
            catch (Exception ex)
            {
                WarningToaster.Toast(App.Current.MainWindow,
                    title: "Ошибка!",
                    message: $"{ex.Message}",
                    position: ToasterPosition.ApplicationBottomRight,
                    animation: ToasterAnimation.SlideInFromRight,
                    margin: 0
                    );
            }
        }

        private async Task ExecuteDeleteFromServiceAsync()
        {
            if (MessageBox.Show($"Вы уверены, что хотите удалить книгу {Book.Name}?",
               "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    if (await _bookService.DeleteBookAsync((int)Book.Id!))
                    {
                        SuccessToaster.Toast(App.Current.MainWindow,
                            title: "Успешно!",
                            message: "Вы успешно добавили книгу в библиотеку",
                            position: ToasterPosition.ApplicationBottomRight,
                            animation: ToasterAnimation.SlideInFromRight,
                            margin: 0
                            );
                        await ExecuteGoToProfileAsync();
                    }
                }
                catch (Exception ex)
                {
                    WarningToaster.Toast(App.Current.MainWindow,
                        title: "Ошибка!",
                        message: $"{ex.Message}",
                        position: ToasterPosition.ApplicationBottomRight,
                        animation: ToasterAnimation.SlideInFromRight,
                        margin: 0
                        );
                }
            }
        }

        private async Task ExecuteGoToProfileAsync()
        {
            try
            {
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                App.MainMenuPage.MainMenuFrame.Navigate(new ProfilePage(App.CurrentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Метод добавления книги в библиотеку
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteAddToLibraryAsync()
        {
            AddLibraryDTO library = new AddLibraryDTO
            {
                idUser = App.CurrentUser.Id,
                idBook = Book.Id
            };

            try
            {
                if (await _libraryService.AddLibraryAsync(library))
                {
                    Book = await _bookService.GetBookByIdAsync((int)Book.Id!);
                    App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
                    SetLibraryButtonsVisible();
                    SuccessToaster.Toast(App.Current.MainWindow,
                        title: "Успешно!",
                        message: "Вы успешно добавили книгу в библиотеку",
                        position: ToasterPosition.ApplicationBottomRight,
                        animation: ToasterAnimation.SlideInFromRight,
                        margin: 0
                        );
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// Метод удаления книги из библиотеки
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeleteFromLibraryAsync()
        {
            AddLibraryDTO library = new AddLibraryDTO
            {
                idUser = App.CurrentUser.Id,
                idBook = Book.Id
            };

            try
            {
                if (await _libraryService.DeleteLibraryAsync(library))
                {
                    Book = await _bookService.GetBookByIdAsync((int)Book.Id!);
                    App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
                    SetLibraryButtonsVisible();
                    SuccessToaster.Toast(App.Current.MainWindow,
                        title: "Успешно!",
                        message: "Вы успешно удалили книгу из библиотеки",
                        position: ToasterPosition.ApplicationBottomRight,
                        animation: ToasterAnimation.SlideInFromRight,
                        margin: 0
                        );
                }
            }
            catch
            {

            }

        }

        private void SetLibraryButtonsVisible()
        {
            IsBtnDeleteFromLibraryVisible =
                App.CurrentUser.Books?.Any(lib =>
                    lib?.Book != null &&
                    lib.Book.Id == Book.Id)
                    ?? false;
            IsBtnAddToLibraryVisible = !IsBtnDeleteFromLibraryVisible;

            IsBtnWriteReviewVisible =  
                !App.CurrentUser.Reviews?.Any(r =>
                    r?.Book != null &&
                    r.Book.Id == Book.Id)
                    ?? false;
        }

        /// <summary>
        /// Метод получения рейтинга книги
        /// </summary>
        /// <returns></returns>
        private double GetRatingBook(BookDTO book)
        {
            var ratings =  book.Reviews?.Select(r => r.Rating).OfType<byte>().ToList();
            if (ratings!.Count == 0)
                return 0.0;

            double average = ratings.Average(r => r);
            return average;
        }
    }
}
