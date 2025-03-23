using Readify.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Readify.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        /// <summary>
        /// Конструктор с выводом ошибки
        /// </summary>
        /// <param name="error">Ошибка</param>
        public LoginPage(string error)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(error);
        }

        /// <summary>
        /// Обработка набора текста в <see cref="PasswordBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }

        }

        /// <summary>
        /// Метод обработки нажатия на Enter для перехода на следующий <see cref="TextBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var currentBox = sender as TextBox;

            if (e.Key == Key.Enter && sender is TextBox && !string.IsNullOrEmpty(currentBox!.Text))
            {
                MoveNextFocus(currentBox!);
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Метод настройки фокуса на следующем по индексу <see cref="TextBox"/>
        /// </summary>
        /// <param name="currentTextBox">Текущий <see cref="TextBox"/>, на котором стоит фокус</param>
        private void MoveNextFocus(TextBox currentTextBox)
        {
            var next = currentTextBox.TabIndex + 1;
            var nextControl = FindControl(currentTextBox, next);
            nextControl?.Focus();
        }

        /// <summary>
        /// Метод поиска следующего <see cref="TextBox"/> по индексу
        /// </summary>
        /// <param name="current">Текущий <see cref="TextBox"/></param>
        /// <param name="tabIndex">Текущий индекс</param>
        /// <returns>Найденный <see cref="TextBox"/></returns>
        private Control FindControl(TextBox current, int tabIndex)
        {
            DependencyObject parent = current.Parent;
            while (parent != null)
            {
                if (parent is StackPanel stackPanel)
                {
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is TextBox control && control.TabIndex == tabIndex)
                        {
                            return control;
                        }
                        if (child is PasswordBox passwordControl && passwordControl.TabIndex == tabIndex)
                        {
                            return passwordControl;
                        }
                    }
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null!;
        }
    }
}
