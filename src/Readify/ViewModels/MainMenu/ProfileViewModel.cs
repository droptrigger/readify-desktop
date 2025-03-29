using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Subscribe;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Pages.MainMenu.Profile;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
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

        private string _textFollowButton = string.Empty!;

        private int _currentUserSubscribers;
        private int _currentUserSubscribtions;

        private bool _isFollowButtonVisible;
        private bool _isUnfolowButtonVisible;
        private bool _isEditButtonVisible;

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
        public string TextFollowButton
        {
            get => _textFollowButton;
            set => SetField(ref _textFollowButton, value);
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
            get => _isEditButtonVisible;
            set => SetField(ref _isEditButtonVisible, value);
        }

        /// <summary>
        /// Видимость кнопки подписки
        /// </summary>
        public bool IsFollowButtonVisible
        {
            get => _isFollowButtonVisible;
            set => SetField(ref _isFollowButtonVisible, value);
        }

        /// <summary>
        /// Видимость кнопки отписки
        /// </summary>
        public bool IsUnfollowButtonVisible
        {
            get => _isUnfolowButtonVisible;
            set => SetField(ref _isUnfolowButtonVisible, value);
        }

        /// <summary>
        /// Текст для блока с подписчиками
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
        /// Команда срабатывающая при нажатии на "... подписчиков"
        /// </summary>
        public ICommand GoToFollowersPage { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на "... подписок"
        /// </summary>
        public ICommand GoToFollowingPage { get; }

        /// <summary>
        /// Инициализация всех данных пользователя
        /// </summary>
        /// <param name="userDTO"></param>
        public void SetUserValues(UserDTO userDTO)
        {
            CurrentUser = userDTO;

            CurrentUserAvatarBytes = userDTO.AvatarImage!;
            CurrentName = userDTO.Name!;
            CurrentUsername = userDTO.Nickname!;
            CurrentDescriptrion = userDTO.Description!;
            CurrentDescriptrion = userDTO.Description!;

            CurrentSubscribers = userDTO.Subscribers?.Count ?? 0;
            CurrentSubscribtions = userDTO.Subscriptions?.Count ?? 0;

            SetEditButtonVisibility();
            SetTextFollowButton();
            SetVisibilityButtons();
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

            FollowUserCommand = new AsyncRelayCommand(ExecuteFollowUserCommandAsync);
            UnfollowUserCommand = new AsyncRelayCommand(ExecuteUnfollowUserCommandAsync);
            GoToFollowersPage = new AsyncRelayCommand(ExecuteGoToFollowersPageAsync);
            GoToFollowingPage = new AsyncRelayCommand(ExecuteGoToFollowingPageAsync);
            EditUserCommand = new AsyncRelayCommand(ExecuteUpdateUserCommandAsync);
        }

        private async Task ExecuteUpdateUserCommandAsync()
        {
            try
            {
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
                App.MainMenuPage.MainMenuFrame.Navigate(new UpdateUserPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteFollowUserCommandAsync()
        {
            SubscribeDTO subscribeDTO = new SubscribeDTO
            {
                AuthorId = CurrentUser.Id,
                SubscriberId = App.CurrentUser.Id
            };

            try
            {
                SetUserValues(await _userService.FollowToUserAsync(subscribeDTO));
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                if (App.IsMainOnProfile && App.ProfilePage.ProfileFrame.Content is ProfileFollowersPage)
                {
                    ProfileFollowersPage page = App.ProfilePage.ProfileFrame.Content as ProfileFollowersPage;
                    page.UpdateFollowers(CurrentUser);
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteUnfollowUserCommandAsync()
        {
            SubscribeDTO subscribeDTO = new SubscribeDTO
            {
                AuthorId = CurrentUser.Id,
                SubscriberId = App.CurrentUser.Id
            };

            try
            {
                SetUserValues(await _userService.UnfollowForUserAsync(subscribeDTO));
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                if (App.IsMainOnProfile && App.ProfilePage.ProfileFrame.Content is ProfileFollowersPage)
                {
                    ProfileFollowersPage page = App.ProfilePage.ProfileFrame.Content as ProfileFollowersPage;
                    page.UpdateFollowers(CurrentUser);
                }
                
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }

        public async Task ExecuteGoToFollowersPageAsync()
        {
            try
            {
                var currentPage = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;
                currentPage!.ProfileFrame.Navigate(new ProfileFollowersPage(_userService, await _userService.GetUserByIdAsync(CurrentUser.Id)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task ExecuteGoToFollowingPageAsync()
        {
            try
            {
                var currentPage = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;
                currentPage!.ProfileFrame.Navigate(new ProfileFollowingPage(_userService, await _userService.GetUserByIdAsync(CurrentUser.Id)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Метод для 
        /// </summary>
        private void SetEditButtonVisibility()
        {
            if (ApplicationUser.Id == CurrentUser.Id)
                _isEditButtonVisible = true;
            else 
                _isEditButtonVisible = false;
        }

        private void SetTextFollowButton()
        {
            var hasSubscribers = App.CurrentUser.Subscribers?.Any() == true;

            var isSubscribed = hasSubscribers && App.CurrentUser.Subscribers!
                .Any(s => s.Id == CurrentUser?.Id);

            if (isSubscribed)
                TextFollowButton = "Подписаться в ответ";


            else
                TextFollowButton = "Подписаться";

        }

        private void SetVisibilityButtons()
        {
            var hasSubscribers = CurrentUser.Subscribers?.Any() == true;

            var isSubscribed = hasSubscribers && CurrentUser.Subscribers!
                .Any(s => s.Id == App.CurrentUser?.Id);


            IsFollowButtonVisible = !isSubscribed && !IsEditButtonVisible;
            IsUnfollowButtonVisible = isSubscribed && !IsEditButtonVisible;
        }

    }
}
