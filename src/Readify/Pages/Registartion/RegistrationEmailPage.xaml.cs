using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Users;
using Readify.Services;
using Readify.Services.Base;
using Readify.ViewModels.Registration;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Readify.Pages.Registartion
{
    /// <summary>
    /// Логика взаимодействия для RegistrationEmailPage.xaml
    /// </summary>
    public partial class RegistrationEmailPage : UserControl
    {
        private IAuthService _authService = App.ServiceProvider.GetService<IAuthService>()!;
        private IRegistrationService _registrationService = App.ServiceProvider.GetService<IRegistrationService>()!;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="registrationDTO"></param>
        public RegistrationEmailPage(RegistrationDTO registrationDTO)
        {
            InitializeComponent();
            DataContext = new RegistrationEmailViewModel(_authService, _registrationService, registrationDTO);
        }

        private void Digit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.Text = e.Text;
                textBox.CaretIndex = 1;
                MoveFocus(textBox);
            }), DispatcherPriority.Input);
        }

        private void Digit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                var textBox = (TextBox)sender;
                if (textBox.Text.Length == 0)
                {
                    MoveFocusBack(textBox);
                }
            }
        }

        private void MoveFocus(TextBox currentTextBox)
        {
            var index = int.Parse(currentTextBox.Name.Substring(5)) - 1;
            if (index < 5)
            {
                var nextBox = FindName($"Digit{index + 2}") as TextBox;
                nextBox?.Focus();
            }
        }

        private void MoveFocusBack(TextBox currentTextBox)
        {
            var index = int.Parse(currentTextBox.Name.Substring(5)) - 1;
            if (index > 0)
            {
                var prevBox = FindName($"Digit{index}") as TextBox;
                prevBox?.Focus();
            }
        }
    }
}
