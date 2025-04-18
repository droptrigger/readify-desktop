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
    /// Логика взаимодействия для ProfileMainPage.xaml
    /// </summary>
    public partial class ProfileMainPage : UserControl
    {
        private IUserService _userService = App.ServiceProvider.GetService<IUserService>()!;
        private IBookService _bookService = App.ServiceProvider.GetService<IBookService>()!;
        private ILibraryService _libraryService = App.ServiceProvider.GetService<ILibraryService>()!;
        private UserDTO _currentUser;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProfileMainPage(UserDTO user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = new ProfileMainViewModel(_userService, user, _bookService, _libraryService);
        }

        /// <summary>
        /// Метод прокрутки книг и отзывов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HorizontalScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.ExtentWidth > scrollViewer.ViewportWidth)
            {
                scrollViewer.ScrollToHorizontalOffset(
                    scrollViewer.HorizontalOffset + (e.Delta > 0 ? -25 : 25)
                );
                e.Handled = true;
            }
        }
    }
}
