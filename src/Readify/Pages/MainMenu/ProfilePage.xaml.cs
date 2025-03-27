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

        /// <summary>
        /// Профиль показываемого пользователя
        /// </summary>
        public UserDTO CurrentUser
        {
            get => _currentUser;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="userDTO"></param>
        public ProfilePage(UserDTO userDTO)
        {
            InitializeComponent();
            _currentUser = userDTO;
            DataContext = new ProfileViewModel(userDTO);
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

        /// <summary>
        /// Метод, срабатываемый при наведении на надпись с подписчиками.
        /// 
        /// Является workaround'ом для перекраски SVG (Триггеры не перекрашивали SVG)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Followers_MouseEnter(object sender, MouseEventArgs e)
        {
            FollowersSVG.Fill = Brushes.Red;
        }

        /// <summary>
        /// Метод, срабатываемый при убирании курсора с надписи с подписчиками.
        /// 
        /// Является workaround'ом для перекраски SVG (Триггеры не перекрашивали SVG)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Followers_MouseLeave(object sender, MouseEventArgs e)
        {
            FollowersSVG.Fill = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#515355"));
        }
    }
}
