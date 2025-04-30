using Readify.DTO.Books;
using Readify.ViewModels.MainMenu;
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

namespace Readify.Pages.MainMenu
{
    /// <summary>
    /// Логика взаимодействия для DeployBookPage.xaml
    /// </summary>
    public partial class DeployBookPage : Page
    {
        public DeployBookPage(List<GenreDTO> genres)
        {
            InitializeComponent();
            DataContext = new DeployBookViewModel(genres);
        }
    }
}
