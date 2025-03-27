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

        private void NavigateToProfilePage()
        {
            ProfilePage profilePage = new ProfilePage(App.CurrentUser);
            App.ProfilePage = profilePage;
            MainMenuFrame.Navigate(profilePage);
        }

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

        private bool IsMouseOverChildScrollViewer(object originalSource)
        {
            var element = originalSource as DependencyObject;
            while (element != null)
            {
                if (element is ItemsControl itemsControl)
                    return true;
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        private void UserAvatarBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            UserPopup.IsOpen = true;

        }

        private void UserAvatarBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            UserPopup.IsOpen = false;
        }

        private void Popup_MouseEnter(object sender, MouseEventArgs e)
        {
            UserPopup.StaysOpen = true;
        }

        private void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            UserPopup.StaysOpen = false;
            UserPopup.IsOpen = false;
        }
    }
}
