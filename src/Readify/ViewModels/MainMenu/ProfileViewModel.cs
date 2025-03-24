using Readify.DTO.Users;
using Readify.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Readify.ViewModels.MainMenu
{
    public class ProfileViewModel : BaseViewModel
    {
        private byte[] _currentUserAvatarBytes = null!;

        private string _currentUserName = string.Empty!;
        private string _currentUserUsername = string.Empty!;
        private string _currentUserDescription = string.Empty!;

        private int _currentUserSubscribers;
        private int _currentUserSubscribtions;
        private int _currentUserCountReviews;
        private int _currentUserCountLikes;
        private int _currentUserId;

        private bool _isEditButtonVisible;
        private bool _isFollowButtonVisible;
        private bool _isUnfollowButtonVisible;

        /// <summary>
        /// Аватар текущего пользователя
        /// </summary>
        public byte[] CurrentUserAvatarBytes
        {
            get => _currentUserAvatarBytes;
            set => SetField(ref _currentUserAvatarBytes, value);
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string CurrentName
        {
            get => _currentUserName;
            set => SetField(ref _currentUserName, value);
        }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string CurrentUsername
        {
            get => _currentUserUsername;
            set => SetField(ref _currentUserUsername, value);
        }

        /// <summary>
        /// Подписчики пользователя
        /// </summary>
        public int CurrentSubscribers
        {
            get => _currentUserSubscribers;
            set => SetField(ref _currentUserSubscribers, value);
        }

        /// <summary>
        /// Подписки пользователя
        /// </summary>
        public int CurrentSubscribtions
        {
            get => _currentUserSubscribtions;
            set => SetField(ref _currentUserSubscribtions, value);
        }

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string CurrentDescriptrion
        {
            get => _currentUserDescription;
            set => SetField(ref _currentUserDescription, value);
        }

        /// <summary>
        /// Количество отзывов
        /// </summary>
        public int CurrentReviews
        {
            get => _currentUserCountReviews;
            set => SetField(ref _currentUserCountReviews, value);
        }

        /// <summary>
        /// Количество отзывов
        /// </summary>
        public int CurrentLikes
        {
            get => _currentUserCountLikes;
            set => SetField(ref _currentUserCountLikes, value);
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int CurrentId
        {
            get => _currentUserId;
            set => SetField(ref _currentUserId, value);
        }

        /// <summary>
        /// Видимость кнопки редактирования
        /// </summary>
        public bool IsEditButtonVisible
        {
            get => _isEditButtonVisible;
            set => SetField(ref _isEditButtonVisible, value);
        }

        /// <summary>
        /// Видимость кнопки подписки
        /// </summary>
        public bool IsFollowButtonVisible
        {
            get => _isFollowButtonVisible;
            set => SetField(ref _isFollowButtonVisible, value);
        }

        /// <summary>
        /// Видимость кнопки оnписки
        /// </summary>
        public bool IsUnfollowButtonVisible
        {
            get => _isUnfollowButtonVisible;
            set => SetField(ref _isUnfollowButtonVisible, value);
        }

        /// <summary>
        /// Текст для блока с подписками
        /// </summary>
        public string SubsText
        {
            get => $"{CurrentSubscribers} Подписчиков · {CurrentSubscribtions} Подписок";
        }

        /// <summary>
        /// Текст для блока с подписками
        /// </summary>
        public string LikesText
        {
            get => $"{CurrentLikes} Лайков";
        }

        /// <summary>
        /// Текст для блока с подписками
        /// </summary>
        public string ReviewsText
        {
            get => $"{CurrentLikes} Отзывов";
        }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Подписаться"
        /// </summary>
        public ICommand FollowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Отписаться"
        /// </summary>
        public ICommand UnfollowUserCommand { get; }

        /// <summary>
        /// Команда срабатывающая при нажатии на кнопку "Обновить профиль"
        /// </summary>
        public ICommand EditUserCommand { get; }


        private void SetUserValues(UserDTO userDTO)
        {
            CurrentUserAvatarBytes = userDTO.AvatarImage!;
            CurrentName = userDTO.Name!;
            CurrentUsername = userDTO.Nickname!;
            CurrentDescriptrion = userDTO.Description!;
            CurrentDescriptrion = userDTO.Description!;

            CurrentSubscribers = userDTO.Subscribers!.Count;
            CurrentSubscribtions = userDTO.Subscriptions!.Count;
            CurrentReviews = userDTO.Reviews!.Count;
            // CurrentLikes = userDTO.Reviews!.;
            CurrentId = userDTO.Id!;
        }

        private void SetVisibleButtons()
        {
            if (CurrentId == App.CurrentUser.Id)
                IsEditButtonVisible = true;

            // TODO

        }

        public ProfileViewModel()
        {
            SetUserValues(App.CurrentUser);
            SetVisibleButtons();
        }
    }
}
