﻿using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.Registartion;
using Readify.Services.Base;
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
        private readonly IAuthService _authService;

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
        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(ExecuteLoginAsync);
            GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
            HiddenErrorCommand = new RelayCommand(() => IsErrorVisible = false);
        }

        /// <summary>
        /// Конструктор с выводом ошибки
        /// </summary>
        public LoginViewModel(IAuthService authService, string message)
        {
            ShowError(message);

            _authService = authService;
            LoginCommand = new RelayCommand(ExecuteLoginAsync);
            GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
            HiddenErrorCommand = new RelayCommand(() => IsErrorVisible = false);
        }

        /// <summary>
        /// Асинхронный метод входа в аккаунт
        /// </summary>
        private async void ExecuteLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Password))
            {
                ShowError("Заполните все поля");
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
                        MainMenuPage mainMenuPage = new MainMenuPage();
                        App.MainMenuPage = mainMenuPage;
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow?.MainFrame.Navigate(mainMenuPage);
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
