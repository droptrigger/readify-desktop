using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Readify.Pages;

namespace Readify
{
    /// <summary>
    /// Логика для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод перехода на страницу профиля
        /// </summary>
        public void NavigateToMenu()
        {
            MainFrame.Navigate(new MainMenuPage());
        }

        /// <summary>
        /// Метод перехода на страницу входа
        /// </summary>
        public void NavigateToLogin()
        {
            MainFrame.Navigate(new LoginPage());
        }

        /// <summary>
        /// Метод перехода на страницу входа с ошибкой
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public void NavigateToLoginError(string message)
        {
            MainFrame.Navigate(new LoginPage(message));
        }

        /// <summary>
        /// Блокировка перехода между фреймами назад
        /// </summary>
        private void MainFrame_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && !(e.OriginalSource is TextBox || e.OriginalSource is PasswordBox))
            {
                e.Handled = true;
            }
        }
    }
}