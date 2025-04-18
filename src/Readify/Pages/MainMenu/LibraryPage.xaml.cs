using Microsoft.Extensions.DependencyInjection;
using Readify.Services.Base;
using Readify.ViewModels;
using Readify.ViewModels.MainMenu;
using System.Windows.Controls;
using System.Windows.Input;

namespace Readify.Pages.MainMenu
{
    /// <summary>
    /// Логика взаимодействия для LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : UserControl
    {
        private ILibraryService _libraryService = App.ServiceProvider.GetService<ILibraryService>();
        private IBookService _bookService = App.ServiceProvider.GetService<IBookService>();

        /// <summary>
        /// Конструктор
        /// </summary>
        public LibraryPage()
        {
            InitializeComponent();
            DataContext = new LibraryViewModel(_libraryService, _bookService);
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
