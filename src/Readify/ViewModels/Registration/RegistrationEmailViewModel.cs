using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Net.Http;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.Registration
{
    /// <summary>
    /// ViewModel для <see cref="RegistrationEmailPage"/>
    /// </summary>
    class RegistrationEmailViewModel : BaseViewModel
    {
        private string _emailText = "Мы отправили код на адрес ";
        private readonly RegistrationDTO _registrationDTO;
        private readonly RegistrationService _registrationService;
        private readonly AuthService _authService;
        private readonly string[] _code = new string[6];

        private string _errorMessage = string.Empty;
        private bool _isErrorVisible;
        private int _countMails;

        /// <summary>
        /// Полный введенный код подтверждения
        /// </summary>
        public string FullCode => string.Join("", _code);

        /// <summary>
        /// Количество отправленных сообщений
        /// </summary>
        public int CountMails
        {
            get => _countMails;
            set => SetField(ref _countMails, value);
        }

        /// <summary>
        /// Текст, говорящий о том, что на почту было отправлено сообщение
        /// </summary>
        public string EmailText
        {
            get => _emailText;
            set => SetField(ref _emailText, value);
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
        /// Первая цифра кода
        /// </summary>
        public string Code1
        {
            get => _code[0];
            set => SetField(ref _code[0], value);
        }

        /// <summary>
        /// Вторая цифра кода
        /// </summary>
        public string Code2 
        { 
            get => _code[1];
            set => SetField(ref _code[1], value);
        }

        /// <summary>
        /// Третья цифра кода
        /// </summary>
        public string Code3 
        { 
            get => _code[2];
            set => SetField(ref _code[2], value);
        }

        /// <summary>
        /// Четвертая цифра кода
        /// </summary>
        public string Code4 
        { 
            get => _code[3];
            set => SetField(ref _code[3], value);
        }

        /// <summary>
        /// Пятая цифра кода
        /// </summary>
        public string Code5 
        {
            get => _code[4];
            set => SetField(ref _code[4], value);
        }

        /// <summary>
        /// Шестая цифра кода
        /// </summary>
        public string Code6 
        { 
            get => _code[5];
            set => SetField(ref _code[5], value);
        }

        /// <summary>
        /// Команжа, срабатывающая при нажатии на кнопку "Продолжить"
        /// </summary>
        public ICommand ContinueCommand { get; }

        /// <summary>
        /// Команжа, срабатывающая при нажатии на кнопку "Отправить код повторно"
        /// </summary>
        public ICommand SendMailCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Измените свой email"
        /// </summary>
        public ICommand GoToRegisterCommand { get; }

        /// <summary>
        /// Команда обработки нажатия на "Войти"
        /// </summary>
        public ICommand GoToLoginCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="registrationDTO"></param>
        public RegistrationEmailViewModel(RegistrationDTO registrationDTO)
        {
            _registrationService = new RegistrationService();
            _authService = new AuthService();

            _registrationDTO = registrationDTO;
            EmailText += registrationDTO.Email;
            ContinueCommand = new AsyncRelayCommand(ExecuteContinueAsync);
            SendMailCommand = new AsyncRelayCommand(ExecuteSendMailAsync);
            GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
            GoToLoginCommand = new RelayCommand(ExecuteGoToLogin);
        }

        /// <summary>
        /// Асинхронный метод отправки электронного письма на почту пользователя
        /// </summary>
        /// <returns>True - если успешно</returns>
        private async Task<bool> ExecuteSendMailAsync()
        {
            CountMails++;

            if (CountMails > 2)
            {
                ShowError("Повторите попытку позже");
                return false;
            }

            try
            {
                if (await _registrationService.SendMailCodeAsync(_registrationDTO.Email!))
                {
                    CloseError();
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ShowError("Что-то пошло не так, повторите попытку позже");
                return false;
            }

            ShowError("Что-то пошло не так, повторите попытку позже");
            return false;
        }

        /// <summary>
        /// Асинхронный метод регистрации и входа в аккаунт
        /// </summary>
        /// <returns>True - если все успешно</returns>
        private async Task<bool> ExecuteContinueAsync()
        {
            if (FullCode.Length < 5)
            {
                ShowError("Введите код полностью");
                return false;
            }

            _registrationDTO.Code = FullCode;
            _registrationDTO.Name = null;

            try
            {
                if (!await _registrationService.RegistrationAsync(_registrationDTO))
                {
                    ShowError("Неверный код");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ShowError("Неверный код");
                return false;
            }


            LoginDTO loginDTO = new LoginDTO
            {
                EmailOrNickname = _registrationDTO.Email,
                Password = _registrationDTO.Password,
                Device = Settings.Default.Device
            };

            try
            {
                if (await _authService.LoginAsync(loginDTO))
                {
                    CloseError();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.MainFrame.Navigate(new RegistrationUpdatePage());
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            ShowError("Что-то пошло не так :/");
            return false;
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
        /// Метод перехода на страницу <see cref="LoginPage"/>
        /// </summary>
        private void ExecuteGoToRegister()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new RegistrationMainPage(_registrationDTO));
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
