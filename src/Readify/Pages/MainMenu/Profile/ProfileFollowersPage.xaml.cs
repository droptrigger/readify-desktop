using Readify.DTO.Users;
using Readify.Services.Base;
using Readify.ViewModels.MainMenu.Profile;
using UserControl = System.Windows.Controls.UserControl;

namespace Readify.Pages.MainMenu.Profile
{
    /// <summary>
    /// Логика взаимодействия для ProfileFollowersAndFollowingPage.xaml
    /// </summary>
    public partial class ProfileFollowersPage : UserControl
    {
        private UserDTO _currentUser;

        public ProfileFollowersPage(IUserService userService, UserDTO currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            DataContext = new ProfileFollowersViewModel(userService, currentUser);
        }

        public void UpdateFollowers(UserDTO user)
        {
            Followers.ItemsSource = user.Subscribers;
        } 
    }
}
