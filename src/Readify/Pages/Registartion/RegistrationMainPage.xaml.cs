using Readify.DTO.Users;
using Readify.ViewModels.Registration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Readify.Pages.Registartion
{
    /// <summary>
    /// Логика взаимодействия для RegistrationMainPage.xaml
    /// </summary>
    public partial class RegistrationMainPage : UserControl
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RegistrationMainPage()
        {
            InitializeComponent();
            DataContext = new RegistrationMainViewModel();
        }

        /// <summary>
        /// Конструктор для возвращения с <see cref="RegistrationEmailPage"/> при нажатии на "Изменить почту"
        /// </summary>
        /// <param name="registrationDTO">Данные, которые вводил пользователь</param>
        public RegistrationMainPage(RegistrationDTO registrationDTO)
        {
            InitializeComponent();
            DataContext = new RegistrationMainViewModel(registrationDTO);
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
        private TextBox FindControl(TextBox current, int tabIndex)
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
                    }
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null!;
        }
    }
}
