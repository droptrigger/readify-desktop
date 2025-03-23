using Readify.DTO.Users;
using Readify.ViewModels.Registration;
using System.Windows.Controls;

namespace Readify.Pages.Registartion
{
    /// <summary>
    /// Логика взаимодействия для RegistrationCapthaPage.xaml
    /// </summary>
    public partial class RegistrationCapthaPage : UserControl
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="registrationDTO">Данные, которые вводил пользователь</param>
        public RegistrationCapthaPage(RegistrationDTO registrationDTO)
        {
            InitializeComponent();
            DataContext = new RegistrationCaptchaViewModel(registrationDTO);
        }
    }
}
