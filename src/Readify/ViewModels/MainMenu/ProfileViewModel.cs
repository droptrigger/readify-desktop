using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.MainMenu
{
    public class ProfileViewModel : BaseViewModel
    {
        private const string NULL_BOOKS_CURRENT_TEXT = "Вы не добавили ни одной книги";
        private const string NULL_BOOKS_USER_TEXT = "Пользователь не добавил ни одной книги";
        private const string NULL_REVIEWS_CURRENT_TEXT = "Вы не написали ни одного отзыва";
        private const string NULL_REVIEWS_USER_TEXT = "Пользователь не написал ни одного отзыва";

        private UserService _userService;

        private byte[] _currentUserAvatarBytes = null!;

        private string _currentUserName = string.Empty!;
        private string _currentUserUsername = string.Empty!;
        private string _currentUserDescription = string.Empty!;

        private int _currentUserSubscribers;
        private int _currentUserSubscribtions;
        private int _currentUserCountReviews;
        private int _currentUserCountLikes;
        private int _currentUserId;

        private UserDTO _currentUser;

        /// <summary>
        /// Пользователь приложения
        /// </summary>
        public UserDTO ApplicationUser
        {
            get => App.CurrentUser;
        }

        /// <summary>
        /// Текущий пользователь, чья страница показывается
        /// </summary>
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthorDTO AppplicationUserAsAuthor
        {
            get
            {
                return new AuthorDTO
                {
                    Id = ApplicationUser.Id,
                    Nickname = ApplicationUser.Nickname,
                    AvatarImage = ApplicationUser.AvatarImage,
                    Name = ApplicationUser.Name
                };
            }
        }

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
        /// Аватар текущего пользователя
        /// </summary>
        public byte[] CurrentUserAvatarBytes
        {
            get => _currentUserAvatarBytes;
            set => SetField(ref _currentUserAvatarBytes, value);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string CurrentName
        {
            get => _currentUserName;
            set => SetField(ref _currentUserName, value);
        }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string CurrentUsername
        {
            get => _currentUserUsername;
            set => SetField(ref _currentUserUsername, value);
        }

        /// <summary>
        /// Подписчики пользователя
        /// </summary>
        public int CurrentSubscribers
        {
            get => _currentUserSubscribers;
            set => SetField(ref _currentUserSubscribers, value);
        }

        /// <summary>
        /// Подписки пользователя
        /// </summary>
        public int CurrentSubscribtions
        {
            get => _currentUserSubscribtions;
            set => SetField(ref _currentUserSubscribtions, value);
        }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string CurrentDescriptrion
        {
            get => _currentUserDescription == null ? "Чудесный пользователь Readify" : _currentUserDescription;
            set => SetField(ref _currentUserDescription, value);
        }

        /// <summary>
        /// Количество отзывов
        /// </summary>
        public int CurrentReviews
        {
            get => _currentUserCountReviews;
            set => SetField(ref _currentUserCountReviews, value);
        }

        /// <summary>
        /// Количество отзывов
        /// </summary>
        public int CurrentLikes
        {
            get => _currentUserCountLikes;
            set => SetField(ref _currentUserCountLikes, value);
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int CurrentId
        {
            get => _currentUserId;
            set => SetField(ref _currentUserId, value);
        }

        /// <summary>
        /// Видимость кнопки редактирования
        /// </summary>
        public bool IsEditButtonVisible
        {
            get
            {
                if (ApplicationUser.Id == CurrentUser.Id)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Видимость кнопки подписки
        /// </summary>
        public bool IsFollowButtonVisible
        {
            get
            {
                if (!CurrentUser.Subscribers!.Contains(AppplicationUserAsAuthor) && !IsEditButtonVisible)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Видимость кнопки отписки
        /// </summary>
        public bool IsUnfollowButtonVisible
        {
            get
            {
                if (CurrentUser.Subscribers!.Contains(AppplicationUserAsAuthor) && !IsEditButtonVisible)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Текст для блока с подписками
        /// </summary>
        public string Followers
        {
            get => $"{CurrentSubscribers} подписчиков";
        }

        /// <summary>
        /// Текст для блока с подписками
        /// </summary>
        public string Following
        {
            get => $"{CurrentSubscribtions} подписок";
        }


        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Подписаться"
        /// </summary>
        public ICommand FollowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Отписаться"
        /// </summary>
        public ICommand UnfollowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Обновить профиль"
        /// </summary>
        public ICommand EditUserCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на никнейм автора книги
        /// </summary>
        public ICommand ShowAuthorCommand { get; }

        private void SetUserValues(UserDTO userDTO)
        {
            CurrentUser = userDTO;

            CurrentUserAvatarBytes = userDTO.AvatarImage!;
            CurrentName = userDTO.Name!;
            CurrentUsername = userDTO.Nickname!;
            CurrentDescriptrion = userDTO.Description!;
            CurrentDescriptrion = userDTO.Description!;

            CurrentSubscribers = userDTO.Subscribers?.Count ?? 0;
            CurrentSubscribtions = userDTO.Subscriptions?.Count ?? 0;
            CurrentReviews = userDTO.Reviews?.Count ?? 0;
            CurrentId = userDTO.Id!;
        }


        public ProfileViewModel()
        {
            SetUserValues(ApplicationUser);
            _userService = new UserService();

            ShowAuthorCommand = new AsyncRelayCommand<int>(ExecuteShowAuthorAsync);
        }

        public ProfileViewModel(UserDTO userDTO)
        {
            SetUserValues(userDTO);
            _userService = new UserService();

            ShowAuthorCommand = new AsyncRelayCommand<int>(ExecuteShowAuthorAsync);
        }

        public async Task ExecuteShowAuthorAsync(int idAuthor)
        {
            if (idAuthor != CurrentId)
            {
                var getUser = await _userService.GetUserByIdAsync(idAuthor);

                if (getUser == null)
                    MessageBox.Show("Ошибка");

                ProfilePage newProfilePage = new ProfilePage(getUser!);
                App.ProfilePage = newProfilePage;

                var mainMenuPageData = App.MainMenuPage?.DataContext! as MainMenuViewModel;
                mainMenuPageData!.UpdateVisibility();

                App.MainMenuPage!.MainMenuFrame.Navigate(newProfilePage);
            }
        }
    }
}
