using CommunityToolkit.Mvvm.Input;
using Readify.DTO.Subscribe;
using Readify.DTO.Users;
using Readify.Pages.MainMenu.Profile;
using Readify.Pages.MainMenu;
using Readify.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Readify.ViewModels.Base;

namespace Readify.ViewModels.MainMenu.Profile 
{
    class ProfileFollowingViewModel : BaseViewModel
    {
        private IUserService _userService;

        private UserDTO _currentUser = null!;
        private bool _isNullFollowVisible;

        /// <summary>
        /// Текущий пользователь, чья страница открыта
        /// </summary>
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        /// <summary>
        /// Текст для блока, говорящем о том, что у пользователя нет подписчиков
        /// </summary>
        public string NullText
        {
            get
            {
                if (CurrentUser.Id == App.CurrentUser.Id)
                    return $"Вы ни на кого не подписаны";
                else
                    return $"{CurrentUser.Nickname} ни на кого не подписан.";
            }
        }

        /// <summary>
        /// Видимость сообщения о том, что пользователь ни на кого не подписан
        /// </summary>
        public bool IsNullFollowVisible
        {
            get => _isNullFollowVisible;
            set => SetField(ref _isNullFollowVisible, value);
        }

        /// <summary>
        /// Команда, срабатывающая при нажатии на пользователя
        /// </summary>
        public ICommand ShowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Подписаться"
        /// </summary>
        public ICommand FollowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Отписаться"
        /// </summary>
        public ICommand UnfollowUserCommand { get; }

        /// <summary>
        /// Команда, срабатывающая при нажатии на "Перейти в профиль"
        /// </summary>
        public ICommand GoToProfileCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProfileFollowingViewModel(IUserService userService, UserDTO currentUser)
        {
            _userService = userService;
            CurrentUser = currentUser;
            SetNullFollowVisible();

            ShowUserCommand = new AsyncRelayCommand<int>(ExecuteShowUserAsync);
            FollowUserCommand = new AsyncRelayCommand<int>(ExecuteFollowUserCommandAsync);
            UnfollowUserCommand = new AsyncRelayCommand<int>(ExecuteUnfollowUserCommandAsync);
            GoToProfileCommand = new AsyncRelayCommand(ExecuteGoToProfileAsync);
        }

        public async Task ExecuteGoToProfileAsync()
        {
            try
            {
                UserDTO profile = await _userService.GetUserByIdAsync(CurrentUser.Id);
                var currentPage = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;
                currentPage.ProfileFrame.Navigate(new ProfileMainPage(profile));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Метод для перехода на страницу пользователя
        /// </summary>
        /// <param name="idAuthor"></param>
        /// <returns></returns>
        public async Task ExecuteShowUserAsync(int idAuthor)
        {
            if (idAuthor != CurrentUser.Id)
            {
                try
                {
                    var getUser = await _userService.GetUserByIdAsync(idAuthor);

                    if (getUser == null)
                        MessageBox.Show("Ошибка");

                    var mainMenuPageData = App.MainMenuPage?.DataContext! as MainMenuViewModel;
                    mainMenuPageData!.NavigateToProfile(getUser!);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteFollowUserCommandAsync(int idAuthor)
        {
            SubscribeDTO subscribeDTO = new SubscribeDTO
            {
                AuthorId = idAuthor,
                SubscriberId = App.CurrentUser.Id
            };

            try
            {
                await _userService.FollowToUserAsync(subscribeDTO);

                CurrentUser = await _userService.GetUserByIdAsync(CurrentUser.Id);
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                Frame frame = App.MainMenuPage.MainMenuFrame;
                ProfilePage profilePage = frame.Content as ProfilePage;
                ProfileViewModel vm = profilePage.DataContext as ProfileViewModel;

                vm!.SetUserValues(CurrentUser);
                ProfilePage currentPage = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;
                currentPage.ProfileFrame.Navigate(new ProfileFollowingPage(_userService, CurrentUser));

            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteUnfollowUserCommandAsync(int idAuthor)
        {
            SubscribeDTO subscribeDTO = new SubscribeDTO
            {
                AuthorId = idAuthor,
                SubscriberId = App.CurrentUser.Id
            };

            try
            {
                await _userService.UnfollowForUserAsync(subscribeDTO);

                CurrentUser = await _userService.GetUserByIdAsync(CurrentUser.Id);
                App.CurrentUser = await _userService.GetUserByIdAsync(App.CurrentUser.Id);

                Frame frame = App.MainMenuPage.MainMenuFrame;
                ProfilePage profilePage = frame.Content as ProfilePage;
                ProfileViewModel vm = profilePage.DataContext as ProfileViewModel;

                vm!.SetUserValues(CurrentUser);
                ProfilePage currentPage = App.MainMenuPage.MainMenuFrame.Content as ProfilePage;
                currentPage.ProfileFrame.Navigate(new ProfileFollowingPage(_userService, CurrentUser));
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }

        private void SetNullFollowVisible()
        {
            if (CurrentUser.Subscriptions!.Count == 0)
                IsNullFollowVisible = true;

            else
                IsNullFollowVisible = false;
        }
    }
}
