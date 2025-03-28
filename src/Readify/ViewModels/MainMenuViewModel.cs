using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using Readify.ViewModels.MainMenu;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Schema;

namespace Readify.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private Stack<UserDTO> _navigationStack = new Stack<UserDTO>();

        private byte[] _applicationUserAvatarBytes = null!;
        private string _applicationUserUsername = string.Empty!;
        private string _searchText = string.Empty!;

        private bool _isLogoVisibility = true;
        private bool _isBackVisibility = false;

        /// <summary>
        /// Сервис для выхода из аккаунта
        /// </summary>
        private IAuthService _authService;

        /// <summary>
        /// Сервис для работы с пользователями
        /// </summary>
        private IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLogoVisibility
        {
            get => _isLogoVisibility;
            set => SetField(ref _isLogoVisibility, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBackVisibility
        {
            get => _isBackVisibility;
            set => SetField(ref _isBackVisibility, value);
        }

        /// <summary>
        /// Изображение профиля пользователя
        /// </summary>
        public byte[] ApplicationUserAvatarBytes
        {
            get => _applicationUserAvatarBytes;
            set => SetField(ref _applicationUserAvatarBytes, value);
        }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string ApplicationUserUsername
        {
            get => _applicationUserUsername;
            set => SetField(ref _applicationUserUsername, value);
        }

        /// <summary>
        /// Поисковой текст
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set => SetField(ref _searchText, value);
        }

        /// <summary>
        /// Команда обработки нажатия на "Readify" и "Книги"
        /// </summary>
        public ICommand GoToBookPageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Поиск"
        /// </summary>
        public ICommand GoToSearchPageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Библиотека"
        /// </summary>
        public ICommand GoToLibararyPageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Профиль"
        /// </summary>
        public ICommand GoToProfilePageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Выход"
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BackFramePageCommand { get; }

        public MainMenuViewModel(IAuthService authService, IUserService userService)
        {
            _navigationStack.Push(App.CurrentUser);
            _authService = authService;
            _userService = userService;

            ApplicationUserAvatarBytes = App.CurrentUser.AvatarImage!;
            ApplicationUserUsername = App.CurrentUser.Nickname!;
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
            BackFramePageCommand = new AsyncRelayCommand(ExecuteBackFramePageCommandAsync);

            GoToProfilePageCommand = new AsyncRelayCommand(ExecuteGoToProfilePageAsync);
        }

        private async Task ExecuteBackFramePageCommandAsync()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop();
                var previousUser = _navigationStack.Peek();

                App.MainMenuPage.MainMenuFrame.Navigate(new ProfilePage(
                    await _userService.GetUserByIdAsync(previousUser.Id)));

                UpdateVisibility();
            }
        }

        private async Task ExecuteGoToProfilePageAsync()
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

        public void UpdateVisibility()
        {
            IsBackVisibility = _navigationStack.Count > 1;
            IsLogoVisibility = _navigationStack.Count() == 1;
        }

        /// <summary>
        /// Метод выхода из аккаунта
        /// </summary>
        /// <returns>True - если успешно</returns>
        private async Task<bool> ExecuteLogoutAsync()
        {
            try
            {
                if (await _authService.LogoutAsync())
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.MainFrame.Navigate(new LoginPage());
                }

                return false;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        public void NavigateToProfile(UserDTO user)
        {
            _navigationStack.Push(user);

            UpdateVisibility();
            App.MainMenuPage.MainMenuFrame.Navigate(new ProfilePage(user));
        }

    }
}
