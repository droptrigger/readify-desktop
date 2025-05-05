using Microsoft.Extensions.DependencyInjection;
using Readify.DTO.Books;
using Readify.Services.Base;
using Readify.ViewModels;
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
    /// Логика взаимодействия для BookPage.xaml
    /// </summary>
    public partial class BookPage : UserControl
    {
        private ILibraryService _libraryService = App.ServiceProvider.GetService<ILibraryService>();
        private IBookService _bookService = App.ServiceProvider.GetService<IBookService>();
        private IUserService _userService = App.ServiceProvider.GetService<IUserService>();

        public BookPage(BookDTO book)
        {
            InitializeComponent();
            DataContext = new BookViewModel(book, _libraryService, _bookService, _userService);
        }
    }
}
