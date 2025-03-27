using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.MainMenu.Profile
{
    public class ProfileMainViewModel : BaseViewModel
    {
        private const string NULL_BOOKS_CURRENT_TEXT = "Вы не добавили ни одной книги";
        private const string NULL_BOOKS_USER_TEXT = "Пользователь не добавил ни одной книги";
        private const string NULL_REVIEWS_CURRENT_TEXT = "Вы не написали ни одного отзыва";
        private const string NULL_REVIEWS_USER_TEXT = "Пользователь не написал ни одного отзыва";

        private IUserService _userService;

        private UserDTO _currentUser = null!;

        /// <summary>
        /// Текск "Книг не добавлено"
        /// </summary>
        public string NullBooksText
        {
            get
            {
                if (App.CurrentUser.Id == CurrentUser.Id)
                    return NULL_BOOKS_CURRENT_TEXT;
                else return NULL_BOOKS_USER_TEXT;
            }
        }

        /// <summary>
        /// Текск "Отзывов не написано"
        /// </summary>
        public string NullReviewsText
        {
            get
            {
                if (App.CurrentUser.Id == CurrentUser.Id)
                    return NULL_REVIEWS_CURRENT_TEXT;
                else return NULL_REVIEWS_USER_TEXT;
            }
        }

        /// <summary>
        /// Видимость "Книг не добавлено"
        /// </summary>
        public bool IsNullBooksVisible
        {
            get
            {
                if (CurrentUser.Books?.Count == 0)
                    return true;
                else return false;
            }

        }

        /// <summary>
        /// Видимость "Отзывов не написано"
        /// </summary>
        public bool IsNullReviewsVisible
        {
            get
            {
                if (CurrentUser.Reviews?.Count == 0)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Видимость "Показать все" книг
        /// </summary>
        public bool IsShowAllBooksVisible
        {
            get
            {
                if (CurrentUser.Books?.Count > 3)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Видимость "Показать все" отзывов
        /// </summary>
        public bool IsShowAllReviewsVisible
        {
            get
            {
                if (CurrentUser.Reviews?.Count > 3)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Пользователь, чью страницу показывает
        /// </summary>
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на никнейм автора книги
        /// </summary>
        public ICommand ShowUserCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="currentUser"></param>
        public ProfileMainViewModel(IUserService userService, UserDTO currentUser)
        {
            CurrentUser = currentUser;
            _userService = userService;

            ShowUserCommand = new AsyncRelayCommand<int>(ExecuteShowUserAsync);
        }

        /// <summary>
        /// Метод для перехода на страницу пользователя
        /// </summary>
        /// <param name="idAuthor"></param>
        /// <returns></returns>
        public async Task ExecuteShowUserAsync(int idAuthor)
        {
            if (idAuthor != CurrentUser.Id)
            {
                var getUser = await _userService.GetUserByIdAsync(idAuthor);

                if (getUser == null)
                    MessageBox.Show("Ошибка");

                var mainMenuPageData = App.MainMenuPage?.DataContext! as MainMenuViewModel;
                mainMenuPageData!.NavigateToProfile(getUser!);

            }
        }
    }
}
