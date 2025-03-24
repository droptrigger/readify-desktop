using CommunityToolkit.Mvvm.Input;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private byte[] _applicationUserAvatarBytes = null!;
        private string _applicationUserUsername = string.Empty!;
        private string _searchText = string.Empty!;

        /// <summary>
        /// Сервис для выхода из аккаунта
        /// </summary>
        private AuthService _authService;

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
        /// Команда обработки нажатия на "Подписки"
        /// </summary>
        public ICommand GoToSubscriptionsPageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Выход"
        /// </summary>
        public ICommand LogoutCommand { get; }

        public MainMenuViewModel()
        {
            _authService = new AuthService();
            _applicationUserAvatarBytes = App.CurrentUser.AvatarImage!;
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
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

    }
}
