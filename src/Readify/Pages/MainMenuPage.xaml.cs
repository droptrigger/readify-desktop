using Readify.Pages.MainMenu;
using Readify.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Readify.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : UserControl
    {
        /// <summary>
        /// Список скроллов, при наведении мышки на которыз скролл не двигался
        /// </summary>
        private string[] _scrolls =
        {
              "BooksHorizontalScroll",
              "ReviewsHorizontalScroll",
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
            NavigateToProfilePage();
            DataContext = new MainMenuViewModel();
        }

        /// <summary>
        /// Блокировка перехода между фреймами назад
        /// </summary>
        private void MainMenuFrame_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && !(e.OriginalSource is TextBox || e.OriginalSource is PasswordBox))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Логика навигации на страницу профиля
        /// </summary>
        private void NavigateToProfilePage()
        {
            ProfilePage profilePage = new ProfilePage(App.CurrentUser);
            App.ProfilePage = profilePage;
            MainMenuFrame.Navigate(profilePage);
        }

        /// <summary>
        /// Обработка прокрути колесиком на скроллере
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsMouseOverChildScrollViewer(e.OriginalSource))
            {
                e.Handled = false;
                return;
            }

            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset + (e.Delta > 0 ? -25 : 25));
            e.Handled = true;
        }

        /// <summary>
        /// Метод проверки находится ли мышка на каком-нибудь скроллере
        /// </summary>
        /// <param name="originalSource"></param>
        /// <returns></returns>
        private bool IsMouseOverChildScrollViewer(object originalSource)
        {
            var element = originalSource as DependencyObject;
            while (element != null)
            {
                if (element is ScrollViewer sv && _scrolls.Contains(sv.Name))
                    return true;
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        /// <summary>
        /// Метод наведения мышки на область с аватаром
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserAvatarBorder_MouseEnter(object sender, MouseEventArgs e) 
            => UserPopup.IsOpen = true;

        /// <summary>
        /// Метод отведения мышки от области с аватаром
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserAvatarBorder_MouseLeave(object sender, MouseEventArgs e)
            => UserPopup.IsOpen = false;

        /// <summary>
        /// Метод наведения мышки на меню выхода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_MouseEnter(object sender, MouseEventArgs e)
            => UserPopup.StaysOpen = true;
        
        /// <summary>
        /// Метод отведения мышки с меню выхода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            UserPopup.StaysOpen = false;
            UserPopup.IsOpen = false;
        }
    }
}
