using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.DTO.Users;
using Readify.ViewModels.MainMenu;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace Readify.Pages.MainMenu
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl
    {
        private UserDTO _currentUser = null!;

        public UserDTO CurrentUser
        {
            get => _currentUser;
        }

        public ProfilePage(UserDTO userDTO)
        {
            InitializeComponent();
            _currentUser = userDTO;
            DataContext = new ProfileViewModel(userDTO);
        }

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

        private void Followers_MouseEnter(object sender, MouseEventArgs e)
        {
            FollowersSVG.Fill = Brushes.Red;
        }

        private void Followers_MouseLeave(object sender, MouseEventArgs e)
        {
            FollowersSVG.Fill = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#515355"));
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var data = (sender! as TextBlock).DataContext as LibraryDTO;


            var viewModel = DataContext as ProfileViewModel;
            viewModel!.ShowAuthorCommand.Execute(data!.Book!.Author!.Id);
        }
    }
}
