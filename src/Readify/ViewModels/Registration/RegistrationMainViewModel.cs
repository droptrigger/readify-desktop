using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.ViewModels.Base;
using ReadifyChecker;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.Registration
{
    /// <summary>
    /// ViewModel для <see cref="RegistrationMainPage">
    /// </summary>
    public class RegistrationMainViewModel : BaseViewModel
    {
        #region Приватные поля
        
        private const string REQUIRED_TO_FILL_IN_ERROR = "Поле обязательно для заполнения";

        private readonly RegistrationService _registrationService;

        private readonly PasswordChecker _passwordChecker;
        private readonly EmailChecker _emailChecker;
        private readonly UsernameChecker _usernameChecker;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _retryPassword = string.Empty;
        private string _username = string.Empty;

        private string _emailErrorMessage = string.Empty;
        private string _passwordErrorMessage = string.Empty;
        private string _retryPasswordErrorMessage = string.Empty;
        private string _usernameErrorMessage = string.Empty;

        private bool _isEmailErrorVisible;
        private bool _isPasswordErrorVisible;
        private bool _isRetryPasswordErrorVisible;
        private bool _isUsernameErrorVisible;

        #endregion

        #region Публичные поля

        #region Поля ввода текста

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
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
        /// Повторение пароля
        /// </summary>
        public string RetryPassword
        {
            get => _retryPassword;
            set => SetField(ref _retryPassword, value);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        #endregion

        #region Поля сообщений об ошибках

        /// <summary>
        /// Сообщение об ошибке с Email
        /// </summary>
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetField(ref _emailErrorMessage, value);
        }

        /// <summary>
        /// Сообщение об ошибке с паролем
        /// </summary>
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetField(ref _passwordErrorMessage, value);
        }

        /// <summary>
        /// Сообщение об ошибке с повторением пароля
        /// </summary>
        public string RetryPasswordErrorMessage
        {
            get => _retryPasswordErrorMessage;
            set => SetField(ref _retryPasswordErrorMessage, value);
        }

        /// <summary>
        /// Сообщение об ошибке с именем пользователя
        /// </summary>
        public string UsernameErrorMessage
        {
            get => _usernameErrorMessage;
            set => SetField(ref _usernameErrorMessage, value);
        }

        #endregion

        #region Поля видимости ошибок

        /// <summary>
        /// Видимость ошибки Email
        /// </summary>
        public bool IsEmailErrorVisible
        {
            get => _isEmailErrorVisible;
            set => SetField(ref _isEmailErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки пароля
        /// </summary>
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetField(ref _isPasswordErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки повторения пароля
        /// </summary>
        public bool IsRetryPasswordErrorVisible
        {
            get => _isRetryPasswordErrorVisible;
            set => SetField(ref _isRetryPasswordErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки имени пользователя
        /// </summary>
        public bool IsUsernameErrorVisible
        {
            get => _isUsernameErrorVisible;
            set => SetField(ref _isUsernameErrorVisible, value);
        }

        #endregion

        #endregion

        /// <summary>
        /// Команда перехода на страницу авторизации
        /// </summary>
        public ICommand GoToLoginCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопку "Продолжить"
        /// </summary>
        public ICommand ContinueCommand { get; }

        /// <summary>
        /// Команда фокуса на поле ввода электронной почты
        /// </summary>
        public ICommand CloseEmailErrorCommand { get; }

        /// <summary>
        /// Команда фокуса на поле ввода пароля
        /// </summary>
        public ICommand ClosePasswordErrorCommand { get; }

        /// <summary>
        /// Команда фокуса на поле повторного ввода пароля
        /// </summary>
        public ICommand CloseRetryPasswordErrorCommand { get; }

        /// <summary>
        /// Команда фокуса на поле ввода имени пользователя
        /// </summary>
        public ICommand CloseUsernameErrorCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RegistrationMainViewModel()
        {
            _registrationService = new RegistrationService();

            _passwordChecker = new PasswordChecker();
            _emailChecker = new EmailChecker();
            _usernameChecker = new UsernameChecker();

            GoToLoginCommand = new RelayCommand(ExecuteGoToLogin);
            ContinueCommand = new AsyncRelayCommand(ExecuteContinueAsync);

            CloseEmailErrorCommand = new RelayCommand(ExecuteCloseEmailError);
            ClosePasswordErrorCommand = new RelayCommand(ExecuteClosePasswordError);
            CloseRetryPasswordErrorCommand = new RelayCommand(ExecuteCloseRetryPasswordError);
            CloseUsernameErrorCommand = new RelayCommand(ExecuteCloseUsernameError);
        }

        /// <summary>
        /// Конструктор, использующийся для перехода со страницы <see cref="RegistrationEmailPage"/>
        /// </summary>
        /// <param name="registrationDTO">Данные, которые вводил пользователь</param>
        public RegistrationMainViewModel(RegistrationDTO registrationDTO)
        {
            _registrationService = new RegistrationService();

            Email = registrationDTO.Email!;
            Password = registrationDTO.Password!;
            RetryPassword = registrationDTO.Password!;
            Username = registrationDTO.Nickname!;

            _passwordChecker = new PasswordChecker();
            _emailChecker = new EmailChecker();
            _usernameChecker = new UsernameChecker();

            GoToLoginCommand = new RelayCommand(ExecuteGoToLogin);
            ContinueCommand = new AsyncRelayCommand(ExecuteContinueAsync);

            CloseEmailErrorCommand = new RelayCommand(ExecuteCloseEmailError);
            ClosePasswordErrorCommand = new RelayCommand(ExecuteClosePasswordError);
            CloseRetryPasswordErrorCommand = new RelayCommand(ExecuteCloseRetryPasswordError);
            CloseUsernameErrorCommand = new RelayCommand(ExecuteCloseUsernameError);
        }

        /// <summary>
        /// Метод перехода на страницу <see cref="LoginPage"/>
        /// </summary>
        private void ExecuteGoToLogin()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new LoginPage());
        }

        /// <summary>
        /// Метод проверки электронной почты
        /// </summary>
        /// <returns>True - если все проверки пройдены</returns>
        private async Task<bool> CheckEmailAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ShowEmailError(REQUIRED_TO_FILL_IN_ERROR);
                return false;
            }

            if (!_emailChecker.Check(Email))
            {
                ShowEmailError("Неправильный формат почты");
                return false;
            }

            try
            {
                if (await _registrationService.CheckEmailAsync(Email))
                {
                    ShowEmailError("Такой email уже зарегистрирован");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сервера: {ex.Message}");
                return false;
            }

            ExecuteCloseEmailError();
            return true;
        }

        /// <summary>
        /// Метод проверки имени пользователя
        /// </summary>
        /// <returns>True - если все проверки пройдены</returns>
        private async Task<bool> CheckUsernameAsync()
        {
            if (string.IsNullOrEmpty(Username))
            {
                ShowUsernameError(REQUIRED_TO_FILL_IN_ERROR);
                return false;
            }

            if (!_usernameChecker.Check(Username))
            {
                ShowUsernameError("Недопустимое имя пользователя");
                return false;
            }
            try
            {
                if (await _registrationService.CheckUsernameAsync(Username))
                {
                    ShowUsernameError("Имя пользователя уже занято");
                    return false;
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка сервера: {ex.Message}");
                return false;
            }


            ExecuteCloseUsernameError();
            return true;
        }

        /// <summary>
        /// Метод проверки пароля на валидность
        /// </summary>
        /// <returns>True - если пароль валиден</returns>
        private bool CheckPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                ShowPasswordError(REQUIRED_TO_FILL_IN_ERROR);
                return false;
            }

            if (_passwordChecker.Check(Password))
            {
                ExecuteClosePasswordError();
                return true;
            }
            else
            {
                ShowPasswordError("Пароль не соответствует требованиям");
                return false;
            }
            
        }

        /// <summary>
        /// Метод проверки правильности повтора пароля
        /// </summary>
        /// <returns>True - если пароли совпадают</returns>
        private bool CheckRetryPassword()
        {
            if (RetryPassword != Password)
            {
                ShowRetryPasswordError("Пароли не совпадают");
                return false;
            }

            if (string.IsNullOrEmpty(RetryPassword))
            {
                ShowRetryPasswordError(REQUIRED_TO_FILL_IN_ERROR);
                return false;

            }
            else
            {
                ExecuteCloseRetryPasswordError();
                return true;
            }
            
        }

        /// <summary>
        /// Метод перехода на следующий этап регистрации (страница <see cref="RegistrationCapthaPage"/>)
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteContinueAsync()
        {
            var email = await CheckEmailAsync();
            var name = await CheckUsernameAsync();
            var password = CheckPassword();
            var retryPassword = CheckRetryPassword();

            if (password && retryPassword && email && name)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new RegistrationCapthaPage(new RegistrationDTO
                {
                    Email = Email,
                    Password = Password,
                    Nickname = Username,
                    Code = string.Empty
                }));
            }
        }

        /// <summary>
        /// Метод вывода ошибки под полем ввода <see cref="Email"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private void ShowEmailError(string message)
        {
            EmailErrorMessage = $"* {message}";
            IsEmailErrorVisible = true;
        }

        /// <summary>
        /// Метод вывода ошибки под полем ввода <see cref="Password"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private void ShowPasswordError(string message)
        {
            PasswordErrorMessage = $"* {message}";
            IsPasswordErrorVisible = true;
        }

        /// <summary>
        /// Метод вывода ошибки под полем ввода <see cref="RetryPassword"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private void ShowRetryPasswordError(string message)
        {
            RetryPasswordErrorMessage = $"* {message}";
            IsRetryPasswordErrorVisible = true;
        }

        /// <summary>
        /// Метод вывода ошибки под полем ввода <see cref="Username"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private void ShowUsernameError(string message)
        {
            UsernameErrorMessage = $"* {message}";
            IsUsernameErrorVisible = true;
        }

        /// <summary>
        /// Метод скрытия ошибки под поле ввода <see cref="Email"/>
        /// </summary>
        private void ExecuteCloseEmailError()
        {
            IsEmailErrorVisible = false;
        }

        /// <summary>
        /// Метод скрытия ошибки под поле ввода <see cref="Password"/>
        /// </summary>
        private void ExecuteClosePasswordError()
        {
            IsPasswordErrorVisible = false;
        }

        /// <summary>
        /// Метод скрытия ошибки под поле ввода <see cref="RetryPassword"/>
        /// </summary>
        private void ExecuteCloseRetryPasswordError()
        {
            IsRetryPasswordErrorVisible = false;
        }

        /// <summary>
        /// Метод скрытия ошибки под поле ввода <see cref="Username"/>
        /// </summary>
        private void ExecuteCloseUsernameError()
        {
            IsUsernameErrorVisible = false;
        }
    }
}
