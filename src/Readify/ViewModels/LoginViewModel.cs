using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.ViewModels.Base;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels
{
    /// <summary>
    /// ViewModel для LoginPage
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isErrorVisible;

        /// <summary>
        /// Сервис авторизации
        /// </summary>
        private readonly AuthService _authService;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        /// <summary>
        /// Видимость сообщения об ошибке
        /// </summary>
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetField(ref _isErrorVisible, value);
        }

        /// <summary>
        /// Команда обработки нажатия на кнопку "Войти"
        /// </summary>
        public ICommand LoginCommand { get; }
        
        /// <summary>
        /// Команда обработки нажатия на "Забыли пароль?"
        /// </summary>
        public ICommand ForgotPasswordCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на кнопку "Создайте аккаунт"
        /// </summary>
        public ICommand GoToRegisterCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на крестик у блока ошибки
        /// </summary>
        public ICommand HiddenErrorCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(ExecuteLoginAsync);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
            GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
            HiddenErrorCommand = new RelayCommand(() => IsErrorVisible = false);
        }

        /// <summary>
        /// Конструктор с выводом ошибки
        /// </summary>
        public LoginViewModel(string message)
        {
            ShowError(message);

            _authService = new AuthService();
            LoginCommand = new RelayCommand(ExecuteLoginAsync);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
            GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
            HiddenErrorCommand = new RelayCommand(() => IsErrorVisible = false);
        }

        /// <summary>
        /// Асинхронный метод входа в аккаунт
        /// </summary>
        private async void ExecuteLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ShowError("Введите имя пользователя");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ShowError("Введите пароль");
                return;
            }
            else
            {
                CloseError();

                try
                {
                    if (await _authService.LoginAsync(new LoginDTO
                    {
                        EmailOrNickname = Username,
                        Password = Password,
                        Device = Settings.Default.Device
                    }))
                    {
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow?.MainFrame.Navigate(new MainMenuPage());
                    }
                    else
                    {
                        ShowError("Неверное имя пользователя или пароль");
                    }
                }
                catch (HttpRequestException)
                {
                    ShowError($"Отсутствует подключение к серверу");
                }
                catch (Exception ex) 
                { 
                    ShowError($"Error: {ex}");
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteForgotPassword()
        {
            //TODO: Навигация на страницу восстановления пароля
        }

        /// <summary>
        /// Метод перехода на страницу <see cref="RegistrationMainPage"/>
        /// </summary>
        private void ExecuteGoToRegister()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new RegistrationMainPage());
        }

        /// <summary>
        /// Метод для вывода блока ошибки с определенным текстом
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        private void ShowError(string message)
        {
            ErrorMessage = message;
            IsErrorVisible = true;
        }

        /// <summary>
        /// Метод закрытия блока ошибки
        /// </summary>
        private void CloseError() 
        {
            IsErrorVisible = false;
        }
    }
}
