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
        private IUserService _userService;

        private byte[] _currentUserAvatarBytes = null!;

        private string _currentUserName = string.Empty!;
        private string _currentUserUsername = string.Empty!;
        private string _currentUserDescription = string.Empty!;

        private int _currentUserSubscribers;
        private int _currentUserSubscribtions;

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
        /// Инициализация всех данных пользователя
        /// </summary>
        /// <param name="userDTO"></param>
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
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="userService">Сервис управления пользователями</param>
        /// <param name="user">Информация пользователя</param>
        public ProfileViewModel(IUserService userService, UserDTO user)
        {
            SetUserValues(user);
            _userService = userService; 
            _currentUser = user;
        }
    }
}
