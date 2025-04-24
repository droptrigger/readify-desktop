using Microsoft.Extensions.DependencyInjection;
using Readify.DTO;
using Readify.Services.Base;
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
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        private IBookService _bookService = App.ServiceProvider.GetService<IBookService>()!;
        private ILibraryService _libraryService = App.ServiceProvider.GetService<ILibraryService>()!;
        private IUserService _userService = App.ServiceProvider.GetService<IUserService>()!;

        public SearchPage(SearchDTO searchDTO)
        {
            InitializeComponent();
            DataContext = new SearchViewModel(searchDTO, _bookService, _libraryService, _userService);
        }

        private void HorizontalScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.ExtentWidth > scrollViewer.ViewportWidth)
            {
                scrollViewer.ScrollToHorizontalOffset(
                    scrollViewer.HorizontalOffset + (e.Delta > 0 ? -25 : 25)
                );
                e.Handled = true;
            }
        }
    }
}
