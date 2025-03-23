using Readify.DTO.Users;
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

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static UserDTO CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
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
