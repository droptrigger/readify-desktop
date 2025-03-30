using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Library;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels;
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

        /// <summary>
        /// Сервайс провайдер для реализации DI
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static UserDTO CurrentUser { get; set; } = null!;

        /// <summary>
        /// Библиотека пользователя
        /// </summary>
        public static SeeLibrariesDTO CurrentUserLibrary { get; set; } = null!;

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static MainMenuPage MainMenuPage { get; set; } = null!;

        public static MainMenuViewModel DataContextMainMenu
        {
            get => MainMenuPage.DataContext as MainMenuViewModel;
        }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public static object InitProfilePage { get; set; } = null!;

        public static bool IsMainOnProfile
        {
            get
            {
                return MainMenuPage.MainMenuFrame.Content is ProfilePage;
            }
        }

        /// <summary>
        /// Текущая страница профиля
        /// </summary>
        public static ProfilePage ProfilePage 
        {
            get
            {
                return MainMenuPage.MainMenuFrame.Content as ProfilePage;
            } 
        }

        /// <summary>
        /// Метод, который вызывавется при запуске приложения (проверка refresh-токена)
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

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

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRegistrationService, RegistrationService>();
            services.AddSingleton<ILibraryService, LibraryService>();
        }
    }

}
