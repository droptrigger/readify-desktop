using Readify.DTO.Users;
using Readify.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readify.ViewModels.MainMenu.Profile
{
    class ProfileFollowersViewModel : BaseViewModel
    {
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

        public string NullText
        {
            get => $"У {CurrentUser.Nickname} пока нет подписчиков.";
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
        /// Конструктор
        /// </summary>
        public ProfileFollowersViewModel(UserDTO currentUser)
        {
            CurrentUser = currentUser;
            SetNullFollowVisible();
        }

        private void SetNullFollowVisible()
        {
            if (CurrentUser.Subscribers!.Count == 0)
                IsNullFollowVisible = true;

            else
                IsNullFollowVisible = false;
        }

    }
}
