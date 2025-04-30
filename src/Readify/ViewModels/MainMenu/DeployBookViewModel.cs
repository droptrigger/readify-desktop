using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using netoaster;
using Readify.Pages.MainMenu;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.IO;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Books;
using ReadifyChecker.Checkers;
using UglyToad.PdfPig;

namespace Readify.ViewModels.MainMenu
{
    public class DeployBookViewModel : BaseViewModel
    {
        private bool _deployBusy = false;

        private IUserService _userService = App.ServiceProvider.GetService<IUserService>()!;
        private IBookService _bookService = App.ServiceProvider.GetService<IBookService>()!;

        private IFormFile _coverFile = null!;
        private IFormFile _bookFile = null!;

        private int _pageCount;

        private byte[] _coverImageBytes = null!;

        private string _bookName = string.Empty!;
        private List<GenreDTO> _bookGenres = null!;
        private GenreDTO _selectedGenre = null!;

        private string _bookDescription = string.Empty!;

        private string _selectFileText = "Выберите файл книги...";

        private string _bookNameErrorText = string.Empty!;
        private string _bookDescriptionErrorText = string.Empty!;
        private string _bookGenreErrorText = string.Empty!;

        private bool _isBookNameErrorVisible;
        private bool _isBookDescriptionErrorVisible;
        private bool _isGenreErrorVisible;

        private BookNameChecker _bookNameChecker = new BookNameChecker();

        public string SelectFileText
        {
            get => _selectFileText;
            set => SetField(ref _selectFileText, value);
        }

        public List<GenreDTO> Genres 
        { 
            get => _bookGenres;
            set => SetField(ref _bookGenres, value); 
        }

        public GenreDTO SelectedGenre 
        { 
            get => _selectedGenre; 
            set => SetField(ref _selectedGenre, value); 
        }

        /// <summary>
        /// Ошибка под полем ввода имени пользователя
        /// </summary>
        public string BookNameErrorText
        {
            get => _bookNameErrorText;
            set => SetField(ref _bookNameErrorText, value);
        }

        /// <summary>
        /// Ошибка под полем ввода имени пользователя
        /// </summary>
        public string GenreErrorText
        {
            get => _bookGenreErrorText;
            set => SetField(ref _bookGenreErrorText, value);
        }

        /// <summary>
        /// Ошибка под полем ввода имени пользователя (логина)
        /// </summary>
        public string BookDescriptionErrorText
        {
            get => _bookDescriptionErrorText;
            set => SetField(ref _bookDescriptionErrorText, value);
        }

        /// <summary>
        /// Видимость ошибки под полем ввода имени пользователя
        /// </summary>
        public bool IsBookNameErrorVisible
        {
            get => _isBookNameErrorVisible;
            set => SetField(ref _isBookNameErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки под полем ввода имени пользователя
        /// </summary>
        public bool IsGenreErrorVisible
        {
            get => _isGenreErrorVisible;
            set => SetField(ref _isGenreErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки под полем ввода имени пользователя
        /// </summary>
        public bool IsBookDescriptionErrorVisible
        {
            get => _isBookDescriptionErrorVisible;
            set => SetField(ref _isBookDescriptionErrorVisible, value);
        }

        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public byte[] BookCoverBytes
        {
            get => _coverImageBytes;
            set => SetField(ref _coverImageBytes, value);
        }

        /// <summary>
        /// Загруженная фотография профиля
        /// </summary>
        public IFormFile CoverImage
        {
            get => _coverFile;
            set => SetField(ref _coverFile, value);
        }

        /// <summary>
        /// Загруженный файл книги
        /// </summary>
        public IFormFile BookFile
        {
            get => _bookFile;
            set => SetField(ref _bookFile, value);
        }

        /// <summary>
        /// Название книги
        /// </summary>
        public string BookName
        {
            get => _bookName;
            set => SetField(ref _bookName, value);
        }

        /// <summary>
        /// Описание книги
        /// </summary>
        public string BookDescription
        {
            get => _bookDescription;
            set => SetField(ref _bookDescription, value);
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопка "Опубликовать книгу"
        /// </summary>
        public ICommand DeployBookCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопка "Выберите файл"
        /// </summary>
        public ICommand SelectCoverCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопка "Выберите файл"
        /// </summary>
        public ICommand SelectFileCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при наборе текста в поле ввода имени книги
        /// </summary>
        public ICommand CloseNameErrorCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при наборе текста в поле ввода описания книги
        /// </summary>
        public ICommand CloseDescriptionErrorCommand { get; }

        public DeployBookViewModel(List<GenreDTO> genres)
        {
            DeployBookCommand = new AsyncRelayCommand(ExecuteDeployBookCommandAsync);
            SelectCoverCommand = new AsyncRelayCommand(ExecuteSelectCoverAsync);
            SelectFileCommand = new AsyncRelayCommand(ExecuteSelectFileAsync);

            CloseNameErrorCommand = new RelayCommand(CloseNameError);
            CloseDescriptionErrorCommand = new RelayCommand(CloseDescriptionError);
            Genres = genres;
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

        private async Task ExecuteDeployBookCommandAsync()
        {
            if (BookCoverBytes is null)
            {
                WarningToaster.Toast(App.Current.MainWindow,
                    title: "Ошибка",
                    message: "Выберите изображение книги",
                    position: ToasterPosition.ApplicationBottomRight,
                    animation: ToasterAnimation.SlideInFromRight,
                    margin: 0
                    );
            }
            else if (CheckNameText() && CheckDescriptionText() && CheckGenre())
            {
                if (BookFile is null)
                {
                    WarningToaster.Toast(App.Current.MainWindow,
                        title: "Ошибка",
                        message: "Выберите файл книги",
                        position: ToasterPosition.ApplicationBottomRight,
                        animation: ToasterAnimation.SlideInFromRight,
                        margin: 0
                        );
                }
                else
                {
                    if (!_deployBusy)
                    {
                        try
                        {
                            _deployBusy = true;
                            AddBookDTO book = new AddBookDTO
                            {
                                Name = BookName,
                                Description = BookDescription,
                                PageQuantity = _pageCount,
                                IdAuthor = App.CurrentUser.Id,
                                CoverImageFile = CoverImage,
                                FileBook = BookFile!,
                                IdGenre = SelectedGenre.Id,
                                IsPrivate = false
                            };

                            bool success = await _bookService.DeployBookAsync(book);

                            if (success)
                            {
                                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                                var mainMenuPageData = App.MainMenuPage?.DataContext! as MainMenuViewModel;
                                mainMenuPageData!.UpdateVisibility();

                                SuccessToaster.Toast(App.Current.MainWindow,
                                    title: "Успешно!",
                                    message: "Книга успешно обновлена",
                                    position: ToasterPosition.ApplicationBottomRight,
                                    animation: ToasterAnimation.SlideInFromRight,
                                    margin: 0
                                    );

                                await ExecuteGoToProfileAsync();
                            }
                            else
                            {
                                WarningToaster.Toast(App.Current.MainWindow,
                                    title: "Ошибка",
                                    message: "Возникла какая-то ошибка...",
                                    position: ToasterPosition.ApplicationBottomRight,
                                    animation: ToasterAnimation.SlideInFromRight,
                                    margin: 0
                                    );
                            }
                        }
                        catch (Exception ex)
                        {
                            _deployBusy = false;
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Метод для выбора фотографии профиля
        /// </summary>
        private async Task ExecuteSelectCoverAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                Title = "Выберите изображение обложки"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    BookCoverBytes = await File.ReadAllBytesAsync(filePath);

                    CoverImage = new FormFile(
                        new MemoryStream(BookCoverBytes),
                        0,
                        BookCoverBytes.Length,
                        "CoverImageFile",
                        Path.GetFileName(filePath)
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Метод для выбора файла книги
        /// </summary>
        private async Task ExecuteSelectFileAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Выберите файл книги"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    var bytes = await File.ReadAllBytesAsync(filePath);

                    BookFile = new FormFile(
                        new MemoryStream(bytes),
                        0,
                        bytes.Length,
                        "FileBook",
                        Path.GetFileName(filePath)
                    );

                    SelectFileText = Path.GetFileName(filePath);
                    _pageCount = GetPdfPageCount(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки файла: {ex.Message}");
                }
            }
        }


        private bool CheckNameText()
        {
            if (BookName.Length < 2)
            {
                ShowNameError("Минимальное количество символов - 2");
                return false;
            }

            if (BookName.Length > 200)
            {
                ShowNameError("Максимальное количество символов - 200");
                return false;
            }

            if (!_bookNameChecker.Check(BookName))
            {
                ShowNameError("Некорректное название книги");
                return false;
            }

            CloseNameError();
            return true;
        }

        private bool CheckGenre()
        {
            if (SelectedGenre is null)
            {
                ShowGenreError("Выберите жанр книги");
                return false;
            }

            CloseGenreError();
            return true;
        }

        private bool CheckDescriptionText()
        {
            if (BookDescription.Length > 1000)
            {
                ShowDescriprionError("Максимальное количество символов - 1000");
                return false;
            }

            CloseDescriptionError();
            return true;
        }

        private void ShowNameError(string message)
        {
            BookNameErrorText = message;
            IsBookNameErrorVisible = true;
        }

        private void ShowDescriprionError(string message)
        {
            BookDescriptionErrorText = message;
            IsBookDescriptionErrorVisible = true;
        }

        private void ShowGenreError(string message)
        {
            GenreErrorText = message;
            IsGenreErrorVisible = true;
        }

        private void CloseNameError()
        {
            IsBookNameErrorVisible = false;
        }

        private void CloseGenreError()
        {
            IsGenreErrorVisible = false;
        }

        private void CloseDescriptionError()
        {
            IsBookDescriptionErrorVisible = false;
        }

        private int GetPdfPageCount(string filePath)
        {
            using var pdfDocument = PdfDocument.Open(filePath);
            return pdfDocument.NumberOfPages;
        }
    }
}
