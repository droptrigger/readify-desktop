using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using netoaster;
using Readify.DTO.Users;
using Readify.Pages;
using Readify.Pages.MainMenu;
using Readify.Services.Base;
using Readify.ViewModels.Base;
using ReadifyChecker;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Readify.ViewModels.MainMenu
{
    public class UpdateUserViewModel : BaseViewModel
    {
        private UserDTO _currentUser;
        private IFormFile _avatarFile = null!;
        private IRegistrationService _registrationService;
        private IUserService _userService;

        private byte[] _userAvatarBytes = null!;
        private string _userName = string.Empty!;
        private string _userNickname = string.Empty!;
        private string _userDescription = string.Empty!;

        private string _userNameErrorText = string.Empty!;
        private string _userNicknameErrorText = string.Empty!;

        private bool _isUserNameErrorVisible;
        private bool _isUserNicknameErrorVisible;

        private UsernameChecker _usernameChecker = new UsernameChecker();

        /// <summary>
        /// Ошибка под полем ввода имени пользователя
        /// </summary>
        public string UserNameErrorText
        {
            get => _userNameErrorText;
            set => SetField(ref _userNameErrorText, value);
        }

        /// <summary>
        /// Ошибка под полем ввода имени пользователя (логина)
        /// </summary>
        public string UserNicknameErrorText
        {
            get => _userNicknameErrorText;
            set => SetField(ref _userNicknameErrorText, value);
        }
        
        /// <summary>
        /// Видимость ошибки под полем ввода имени пользователя
        /// </summary>
        public bool IsUserNameErrorVisible
        {
            get => _isUserNameErrorVisible;
            set => SetField(ref _isUserNameErrorVisible, value);
        }

        /// <summary>
        /// Видимость ошибки под полем ввода имени пользователя
        /// </summary>
        public bool IsUserNicknameErrorVisible
        {
            get => _isUserNicknameErrorVisible;
            set => SetField(ref _isUserNicknameErrorVisible, value);
        }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public UserDTO CurentUser
        {
            get => _currentUser;
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId
        {
            get => CurentUser.Id;
        }

        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public byte[] UserAvatarBytes
        {
            get => _userAvatarBytes;
            set => SetField(ref _userAvatarBytes, value);
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
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
        }

        /// <summary>
        /// Имя пользователя (Логин)
        /// </summary>
        public string UserNickname
        {
            get => _userNickname;
            set => SetField(ref _userNickname, value);
        }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string UserDescription
        {
            get => _userDescription;
            set => SetField(ref _userDescription, value);
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопка "Обновить профиль"
        /// </summary>
        public ICommand UpdateProfileCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на кнопка "Обновить профиль"
        /// </summary>
        public ICommand SelectAvatarCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при наборе текста в поле ввода имени
        /// </summary>
        public ICommand CloseNameErrorCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при наборе текста в поле ввода имени пользователя
        /// </summary>
        public ICommand CloseNicknameErrorCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на "Вернуться в профиль"
        /// </summary>
        public ICommand GoToProfileCommand { get; }

        public UpdateUserViewModel(IRegistrationService registrationService, IUserService userService)
        {
            _registrationService = registrationService;
            _userService = userService;
            _currentUser = App.CurrentUser;

            UserAvatarBytes = CurentUser.AvatarImage!;
            UserName = CurentUser.Name!;
            UserNickname = CurentUser.Nickname!;
            UserDescription = CurentUser.Description!;

            UpdateProfileCommand = new AsyncRelayCommand(ExecuteUpdateProfileCommandAsync);
            SelectAvatarCommand = new AsyncRelayCommand(ExecuteSelectAvatarAsync);
            GoToProfileCommand = new AsyncRelayCommand(ExecuteGoToProfileAsync);

            CloseNameErrorCommand = new RelayCommand(CloseNameError);
            CloseNicknameErrorCommand = new RelayCommand(CloseNicknameError);
        }

        private async Task ExecuteGoToProfileAsync()
        {
            try
            {
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);
           
                App.MainMenuPage.MainMenuFrame.Navigate(new ProfilePage(App.CurrentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async Task ExecuteUpdateProfileCommandAsync()
        {
            if (CheckNameText() && await CheckNicknameText())
            {
                try
                {
                    UpdateUserDTO update = new UpdateUserDTO
                    {
                        UserId = UserId,
                        Nickname = UserNickname,
                        Name = UserName,
                        Description = UserDescription,
                        AvatarImage = AvatarImage
                    };

                    bool success = await _registrationService.UpdateUserAsync(update);

                    if (success)
                    {
                        App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                        var mainMenuPageData = App.MainMenuPage?.DataContext! as MainMenuViewModel;
                        mainMenuPageData!.UpdateVisibility();

                        SuccessToaster.Toast(App.Current.MainWindow, 
                            title: "Успешно!", 
                            message: "Данные успешно обновлены", 
                            position: ToasterPosition.ApplicationBottomRight, 
                            animation: ToasterAnimation.SlideInFromRight,
                            margin: 0
                            );
                    }
                    else
                    {
                        WarningToaster.Toast(App.Current.MainWindow,
                            title: "Ошибка",
                            message: "Возникла какая-то ошибка...",
                            position: ToasterPosition.ApplicationBottomRight,
                            animation: ToasterAnimation.SlideInFromRight,
                            margin: 0
                            );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
                    UserAvatarBytes = await File.ReadAllBytesAsync(filePath);

                    AvatarImage = new FormFile(
                        new MemoryStream(UserAvatarBytes),
                        0,
                        UserAvatarBytes.Length,
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

        private bool CheckNameText()
        {
            if (UserName.Length < 2)
            {
                ShowNameError("Минимальное количество символов - 2");
                return false;
            }
            
            if (UserName.Length > 100)
            {
                ShowNameError("Максимальное количество символов - 100");
                return false;
            }

            CloseNameError();
            return true;
        }

        private async Task<bool> CheckNicknameText()
        {
            if (UserNickname.Length < 5)
            {
                ShowNicknameError("Минимальное количество символов - 5");
                return false;
            }

            if (UserNickname.Length > 50)
            {
                ShowNicknameError("Максимальное количество символов - 50");
                return false;
            }

            if (UserNickname != App.CurrentUser.Nickname && await _registrationService.CheckUsernameAsync(UserNickname))
            {
                ShowNicknameError("Имя пользователя уже занято");
                return false;
            }

            if (!_usernameChecker.Check(UserNickname))
            {
                ShowNicknameError("Некорректное имя пользователя");
                return false;
            }

            CloseNicknameError();
            return true;
        }

        private void ShowNameError(string message)
        {
            UserNameErrorText = message;
            IsUserNameErrorVisible = true;
        }

        private void ShowNicknameError(string message)
        {
            UserNicknameErrorText = message;
            IsUserNicknameErrorVisible = true;
        }

        private void CloseNameError()
        {
            IsUserNameErrorVisible = false;
        }

        private void CloseNicknameError()
        {
            IsUserNicknameErrorVisible = false;
        }
    }
}
