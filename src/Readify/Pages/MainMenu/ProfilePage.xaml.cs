using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Users;
using Readify.Pages.MainMenu.Profile;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.MainMenu;
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
        private IUserService _userService = App.ServiceProvider.GetService<IUserService>()!;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        public ProfilePage(UserDTO user)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(_userService, user);
            ProfileFrame.Navigate(new ProfileMainPage(user));
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
