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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Schema;

namespace Readify.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private Stack<object> _navigationStack = new Stack<object>();

        private byte[] _applicationUserAvatarBytes = null!;
        private string _applicationUserUsername = string.Empty!;
        private string _searchText = string.Empty!;

        private bool _isLogoVisibility = true;
        private bool _isBackVisibility = false;

        /// <summary>
        /// Стэк открытых страниц внутри фрейма
        /// </summary>
        public Stack<object> NavigationStack 
        {
            get => _navigationStack;
            set => SetField(ref _navigationStack, value);
        }

        /// <summary>
        /// Сервис для выхода из аккаунта
        /// </summary>
        private IAuthService _authService;

        /// <summary>
        /// Сервис для работы с пользователями
        /// </summary>
        private IUserService _userService;

        /// <summary>
        /// Сервис для работы с библиотекой
        /// </summary>
        private ILibraryService _libraryService;

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

        public MainMenuViewModel(IAuthService authService, IUserService userService, ILibraryService libraryService)
        {
            NavigationStack.Push(App.InitProfilePage);

            _authService = authService;
            _userService = userService;
            _libraryService = libraryService;

            ApplicationUserAvatarBytes = App.CurrentUser.AvatarImage!;
            ApplicationUserUsername = App.CurrentUser.Nickname!;
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
            BackFramePageCommand = new AsyncRelayCommand(ExecuteBackFramePageCommandAsync);

            GoToProfilePageCommand = new AsyncRelayCommand(ExecuteGoToProfilePageAsync);
            GoToLibararyPageCommand = new AsyncRelayCommand(ExecuteGoToLibraryPageAsync);
        }

        private async Task ExecuteBackFramePageCommandAsync()
        {
            var page = NavigationStack.Peek();

            if (page is ProfilePage)
            {
                ProfilePage profile = (ProfilePage)page;
                ProfileViewModel profileVM = (ProfileViewModel)profile.DataContext;

                if (profileVM.NavigationStack.Count > 1)
                    profileVM?.GoBackFrameAsync();

                else
                {
                    if (NavigationStack.Count > 1)
                        NavigationStack.Pop();

                    page = NavigationStack.Peek();
                    profile = (ProfilePage)page;
                    profileVM = (ProfileViewModel)profile.DataContext;
                    profileVM.CurrentUser = await _userService.GetUserByIdAsync(profileVM.CurrentUser.Id);

                    App.MainMenuPage.MainMenuFrame.Navigate(profile);
                }

                UpdateVisibility();
            }
            if (page is UpdateUserPage)
            {
                await ExecuteGoToProfilePageAsync();
            }
        }

        private async Task ExecuteGoToProfilePageAsync()
        {
            try
            {
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
                ProfilePage profile = new ProfilePage(App.CurrentUser);

                NavigationStack.Clear();
                NavigationStack.Push(profile);
                App.InitProfilePage = profile;

                App.MainMenuPage.MainMenuFrame.Navigate(profile);
                UpdateVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ExecuteGoToLibraryPageAsync()
        {
            try
            {
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
                App.CurrentUserLibrary = await _libraryService.GetBooksByUserIdAsync(App.CurrentUser.Id);
                LibraryPage library = new LibraryPage();

                NavigationStack.Clear();
                NavigationStack.Push(library);
                App.InitProfilePage = library;

                App.MainMenuPage.MainMenuFrame.Navigate(library);
                UpdateVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateVisibility()
        {
            var page = NavigationStack.Peek();
            bool goBack = false;

            if (page is ProfilePage)
            {
                ProfilePage profilePage = (ProfilePage)page;
                ProfileViewModel profileViewModel = (ProfileViewModel)profilePage.DataContext;
                goBack = profileViewModel.NavigationStack.Count > 1;
            }

            IsBackVisibility = NavigationStack.Count > 1 || goBack;

            IsLogoVisibility = !IsBackVisibility;

            ApplicationUserAvatarBytes = App.CurrentUser.AvatarImage!;
            ApplicationUserUsername = App.CurrentUser.Nickname!;
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
            ProfilePage profile = new ProfilePage(user);
            App.InitProfilePage = profile;

            NavigationStack.Push(profile);

            App.MainMenuPage.MainMenuFrame.Navigate(profile);
            UpdateVisibility();
        }

    }
}
