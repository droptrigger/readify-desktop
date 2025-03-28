using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Users;
using Readify.Services.Base;
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
    /// Логика взаимодействия для ProfileFollowingPage.xaml
    /// </summary>
    public partial class ProfileFollowingPage : UserControl
    {
        public ProfileFollowingPage(IUserService userService, UserDTO user)
        {
            InitializeComponent();
            DataContext = new ProfileFollowingViewModel(userService, user);
        }
    }
}
