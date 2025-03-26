using CommunityToolkit.Mvvm.Input;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.ViewModels.Base;
using Readify.ViewModels.MainMenu;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Schema;

namespace Readify.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private byte[] _applicationUserAvatarBytes = null!;
        private string _applicationUserUsername = string.Empty!;
        private string _searchText = string.Empty!;

        private bool _isLogoVisibility = true;
        private bool _isBackVisibility = false;

        /// <summary>
        /// Сервис для выхода из аккаунта
        /// </summary>
        private AuthService _authService;

        private ProfileViewModel DataContextProfilePage
        {
            get => App.ProfilePage?.DataContext! as ProfileViewModel;
        }

        public bool IsLogoVisibility
        {
            get => _isLogoVisibility;
            set => SetField(ref _isLogoVisibility, value);
        }

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
        /// Команда обработки нажатия на "Подписки"
        /// </summary>
        public ICommand GoToSubscriptionsPageCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Выход"
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BackFramePageCommand { get; }

        public MainMenuViewModel()
        {
            _authService = new AuthService();
            ApplicationUserAvatarBytes = App.CurrentUser.AvatarImage!;
            ApplicationUserUsername = App.CurrentUser.Nickname!;
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
            BackFramePageCommand = new RelayCommand(ExecuteBackFramePageCommand);
        }

        private void ExecuteBackFramePageCommand()
        {
            if (App.MainMenuPage?.MainMenuFrame?.CanGoBack ?? false)
            {
                App.MainMenuPage.MainMenuFrame.GoBack();
                var page = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;

                App.ProfilePage = page;
                DataContextProfilePage.CurrentUser = page!.CurrentUser;
                UpdateVisibility();
            }
        }

        public void UpdateVisibility()
        {
            bool isCurrentUser = DataContextProfilePage.CurrentUser.Id == App.CurrentUser.Id;
            IsLogoVisibility = isCurrentUser;
            IsBackVisibility = !isCurrentUser;

            MessageBox.Show(DataContextProfilePage.CurrentUser.Id + " " + App.CurrentUser.Id);
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
