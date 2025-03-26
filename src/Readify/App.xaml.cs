using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Services;
using System.Net.Http;
using System.Windows;

namespace Readify
{
    /// <summary>
    /// Логика для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Сервис аутентификации
        /// </summary>
        private AuthService _authService = new AuthService();

        private static UserDTO _currentUser = null!;

        private static MainMenuPage _mainMenuPage = null!;

        private static ProfilePage _profilePage = null!;

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static UserDTO CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static MainMenuPage MainMenuPage
        {
            get => _mainMenuPage;
            set => _mainMenuPage = value;
        }


        /// <summary>
        /// Текущая страница профиля
        /// </summary>
        public static ProfilePage ProfilePage
        {
            get => _profilePage;
            set => _profilePage = value;
        }

        /// <summary>
        /// Метод, который вызывавется при запуске приложения
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            if (!string.IsNullOrEmpty(Settings.Default.RefreshToken))
            {
                try
                {
                    if (await _authService.RefreshTokenAsync())
                    {
                        mainWindow.NavigateToMenu();
                    }
                    else
                    {
                        mainWindow.NavigateToLogin();
                    }
                }
                catch (HttpRequestException)
                {
                    mainWindow.NavigateToLoginError("Отсутствует соединение с сервером");
                }
            }
            else
            {
                mainWindow.NavigateToLogin();
            }

            mainWindow.Show();
        }
    }

}
