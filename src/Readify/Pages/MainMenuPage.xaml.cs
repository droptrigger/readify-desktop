using Readify.Pages.MainMenu;
using Readify.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

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
            MainMenuFrame.Navigate(new ProfilePage());
        }
    }
}
