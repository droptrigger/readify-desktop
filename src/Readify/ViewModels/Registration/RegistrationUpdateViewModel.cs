using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.Registration
{
    /// <summary>
    /// ViewModel для <see cref=""/>
    /// </summary>
    class RegistrationUpdateViewModel : BaseViewModel
    {
        private RegistrationService _registrationService;

        private byte[] _avatarBytes = null!;
        private IFormFile _avatarFile = null!;

        private string _name = string.Empty;
        private string _about = string.Empty;

        private string _nameErrorMessage = string.Empty;
        private string _aboutErrorMessage = string.Empty;

        private bool _isNameErrorVisible;
        private bool _isAboutErrorVisible;

        public byte[] AvatarBytes
        {
            get => _avatarBytes;
            set => SetField(ref _avatarBytes, value);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string About
        {
            get => _about;
            set => SetField(ref _about, value);
        }

        /// <summary>
        /// Текст ошибки, связанной с именем пользователя
        /// </summary>
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetField(ref _nameErrorMessage, value);
        }

        /// <summary>
        /// Текст ошибки, связаной с описанием пользователя
        /// </summary>
        public string AboutErrorMessage
        {
            get => _aboutErrorMessage;
            set => SetField(ref _aboutErrorMessage, value);
        }

        /// <summary>
        /// Видимость ошибки имени пользователя
        /// </summary>
        public bool IsNameErrorVisible
        {
            get => _isNameErrorVisible;
            set => SetField(ref _isNameErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки описания пользователя
        /// </summary>
        public bool IsAboutErrorVisible
        {
            get => _isAboutErrorVisible;
            set => SetField(ref _isAboutErrorVisible, value);
        }

        /// <summary>
        /// Загруженная фотография профиля
        /// </summary>
        public IFormFile AvatarImage
        {
            get => _avatarFile;
            set => SetField(ref _avatarFile, value);
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на "Пропустить"
        /// </summary>
        public ICommand GoToProfilePageCommand { get; }
        
        /// <summary>
        /// Команда, срабатывающая при нажатии на поле выбора аватара
        /// </summary>
        public ICommand SelectAvatarCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на "Звершить регистрацию"
        /// </summary>
        public ICommand FinishRegistrationCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при фокусе на поле ввода <see cref="Name"/>
        /// </summary>
        public ICommand CloseNameErrorCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при фокусе на поле ввода <see cref="About"/>
        /// </summary>
        public ICommand CloseAboutErrorCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RegistrationUpdateViewModel()
        {
            Name = App.CurrentUser.Name!;

            _registrationService = new RegistrationService();

            CloseNameErrorCommand = new RelayCommand(CloseNameError);
            CloseAboutErrorCommand = new RelayCommand(CloseAboutError);

            GoToProfilePageCommand = new RelayCommand(ExecuteGoToProfilePage);
            SelectAvatarCommand = new AsyncRelayCommand(ExecuteSelectAvatarAsync);
            FinishRegistrationCommand = new AsyncRelayCommand(ExecuteFinishRegistrationAsync);
        }

        /// <summary>
        /// Метод перехода на страницу профиля
        /// </summary>
        private void ExecuteGoToProfilePage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainMenuPage());
        }

        /// <summary>
        /// Метод для выбора фотографии профиля
        /// </summary>
        private async Task ExecuteSelectAvatarAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                Title = "Выберите изображение профиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    AvatarBytes = await File.ReadAllBytesAsync(filePath);

                    AvatarImage = new FormFile(
                        new MemoryStream(AvatarBytes),
                        0,
                        AvatarBytes.Length,
                        "avatarImage",
                        Path.GetFileName(filePath)
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Метод проверки имени пользователя
        /// </summary>
        /// <returns>True - если успешно</returns>
        private bool CheckName()
        {
            if (Name.Length < 2)
            {
                ShowNameError("Минимально допустимое значение символов: 2");
                return false;
            }

            if (Name.Length > 100)
            {
                ShowNameError("Максимально допустимое значение символов: 100");
                return false;
            }

            CloseNameError();
            return true;
        }

        /// <summary>
        /// Метод проверки описания
        /// </summary>
        /// <returns>True - если успешно</returns>
        private bool CheckAbout()
        {
            if (Name.Length > 250)
            {
                ShowAboutError("Максимально допустимое значение символов: 250");
                return false;
            }

            CloseAboutError();
            return true;
        }

        /// <summary>
        /// Метод завершения регистрации
        /// </summary>
        /// <returns>True - если успешно</returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<bool> ExecuteFinishRegistrationAsync()
        {
            bool boolName = CheckName();
            bool boolAbout = CheckAbout();

            if (!boolName || !boolAbout)
            {
                return false;
            }

            try
            {
                if (await _registrationService.UpdateUserAsync(new UpdateUserDTO
                {
                    UserId = App.CurrentUser.Id,
                    Nickname = App.CurrentUser.Nickname,
                    Description = About,
                    AvatarImage = AvatarImage,
                    Name = Name,
                }))
                {
                    ExecuteGoToProfilePage();
                    return true;
                }
                else
                {
                    ShowNameError("Ошибка :/");
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                App.CurrentUser = null!;

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new LoginPage("Время сессии истекло"));

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Метод показа ошибки под полем ввода <see cref="Name">
        /// </summary>
        /// <param name="message"></param>
        private void ShowNameError(string message)
        {
            NameErrorMessage = message;
            IsNameErrorVisible = true;
        }

        /// <summary>
        /// Метод показа ошибки под полем ввода <see cref="About">
        /// </summary>
        /// <param name="message"></param>
        private void ShowAboutError(string message)
        {
            AboutErrorMessage = message;
            IsAboutErrorVisible = true;
        }

        /// <summary>
        /// Метод скрытия ошибки под полем ввода <see cref="Name">
        /// </summary>
        private void CloseNameError()
        {
            IsNameErrorVisible = false;
        }

        /// <summary>
        /// Метод скрытия ошибки под полем ввода <see cref="About">
        /// </summary>
        private void CloseAboutError()
        {
            IsAboutErrorVisible = false;
        }
    }
}
