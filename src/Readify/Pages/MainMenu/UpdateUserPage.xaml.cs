using Microsoft.Extensions.DependencyInjection;
using Readify.Services.Base;
using Readify.ViewModels.MainMenu;
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

namespace Readify.Pages.MainMenu
{
    /// <summary>
    /// Логика взаимодействия для UpdateUserPage.xaml
    /// </summary>
    public partial class UpdateUserPage : UserControl
    {
        private IRegistrationService _registrationService = App.ServiceProvider.GetService<IRegistrationService>()!;
        private IUserService _userService = App.ServiceProvider.GetService<IUserService>()!;

        public UpdateUserPage()
        {
            InitializeComponent();
            DataContext = new UpdateUserViewModel(_registrationService, _userService);
        }
    }
}
