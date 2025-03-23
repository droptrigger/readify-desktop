using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
