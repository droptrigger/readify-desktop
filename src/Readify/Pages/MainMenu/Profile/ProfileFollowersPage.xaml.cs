using Readify.DTO.Users;
using Readify.ViewModels.MainMenu.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Readify.Pages.MainMenu.Profile
{
    /// <summary>
    /// Логика взаимодействия для ProfileFollowersAndFollowingPage.xaml
    /// </summary>
    public partial class ProfileFollowersPage : UserControl
    {
        private UserDTO _currentUser;

        public ProfileFollowersPage(UserDTO currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            DataContext = new ProfileFollowersViewModel(currentUser);
        }
    }
}
