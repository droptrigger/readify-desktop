using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.Registration
{
    /// <summary>
    /// ViewModel для <see cref="RegistrationCapthaPage"/>
    /// </summary>
    public class RegistrationCaptchaViewModel : BaseViewModel
    {
        private const string NOT_EQUALS_CAPTCHA_ERROR = "Попробуйте еще раз";

        private readonly IRegistrationService _registrationService;
        private readonly RegistrationDTO _registrationDTO;

        private string _captcha = string.Empty;
        private string _captchaErrorMessage = string.Empty;
        private string _captchaText = string.Empty;

        private bool _isCaptchaErrorVisible;

        public string Captcha
        {
            get => _captcha;
            set => SetField(ref _captcha, value);
        }

        public string CaptchaText
        {
            get => _captchaText;
            set => SetField(ref _captchaText, value);
        }

        public string CaptchaErrorMessage
        {
            get => _captchaErrorMessage;
            set => SetField(ref _captchaErrorMessage, value);
        }

        public bool IsCaptchaErrorVisible
        {
            get => _isCaptchaErrorVisible;
            set => SetField(ref _isCaptchaErrorVisible, value);
        }

        /// <summary>
        /// Команда перехода на страницу авторизации
        /// </summary>
        public ICommand GoToLoginCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопку "Продолжить"
        /// </summary>
        public ICommand ContinueCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопку "Продолжить"
        /// </summary>
        public ICommand CloseCaptchaErrorCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RegistrationCaptchaViewModel(IRegistrationService registrationService, RegistrationDTO registrationDTO)
        {
            _registrationService = registrationService;
            _registrationDTO = registrationDTO;

            CloseCaptchaErrorCommand = new RelayCommand(CloseCaptchaError);
            GoToLoginCommand = new RelayCommand(ExecuteGoToLogin);
            ContinueCommand = new AsyncRelayCommand(ExecuteContinueAsync);

            CaptchaText = GenerateRandomText(6);
        }

        /// <summary>
        /// Генерация капчи
        /// </summary>
        /// <param name="length">Длина капчи</param>
        /// <returns>Строка со случайными символами</returns>
        private string GenerateRandomText(int length)
        {
            string _chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz0123456789";
            Random _random = new Random();

            var chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = _chars[_random.Next(_chars.Length)];
            return new string(chars);
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
        /// Метод перехода на следующий этап регистрации (страница <see cref=""/>)
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteContinueAsync()
        {
            if (Captcha != CaptchaText)
            {
                Captcha = string.Empty;
                ShowCapthcaError(NOT_EQUALS_CAPTCHA_ERROR);
                CaptchaText = GenerateRandomText(6);
            }
            else
            {
                try
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.MainFrame.Navigate(new RegistrationEmailPage(_registrationDTO));
                    await _registrationService.SendMailCodeAsync(_registrationDTO.Email!);
                    CloseCaptchaError();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Ошибка сервера {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Метод скрытия ошибки под поле ввода <see cref="Captcha"/>
        /// </summary>
        private void CloseCaptchaError()
        {
            IsCaptchaErrorVisible = false;
        }

        /// <summary>
        /// Метод вывода ошибки под полем ввода <see cref="Captcha"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private void ShowCapthcaError(string message)
        {
            CaptchaErrorMessage = $"* {message}";
            IsCaptchaErrorVisible = true;
        }

    }
}
