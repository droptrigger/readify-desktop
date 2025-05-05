using Readify.DTO.Books;
using PdfLibCore;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using Readify.ViewModels.Base;

namespace Readify.ViewModels.MainMenu
{
    public class PDFViewModel : BaseViewModel
    {
        private PdfDocument _pdfDocument;
        private int _currentPageIndex;
        private BitmapImage _currentPageImage;
        private BookDTO _currentBook;
        private string _pageInfo;

        public BookDTO CurrentBook
        {
            get => _currentBook;
            set => SetField(ref _currentBook, value);
        }

        public BitmapImage CurrentPageImage
        {
            get => _currentPageImage;
            set => SetField(ref _currentPageImage, value);
        }

        public string PageInfo
        {
            get => _pageInfo;
            set => SetField(ref _pageInfo, value);
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public PDFViewModel()
        {
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
        }

        public void LoadBook(BookDTO book)
        {
            CurrentBook = book;

            if (book?.FileBook == null || book.FileBook.Length == 0)
                throw new InvalidOperationException("PDF file is empty or not available");

            try
            {
                _pdfDocument = new PdfLibCore.PdfDocument(book.FileBook);
                _currentPageIndex = 0;
                LoadPage(_currentPageIndex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load PDF document", ex);
            }
        }

        private void LoadPage(int pageIndex)
        {
            if (_pdfDocument == null || pageIndex < 0 || pageIndex >= _pdfDocument.Pages.Count)
                return;

            _currentPageIndex = pageIndex;
            using var page = _pdfDocument.Pages[pageIndex];

            using var stream = new MemoryStream();
            using var pdfBitmap = new PdfiumBitmap(
                (int)page.Width,
                (int)page.Height,
                true);

            page.Render(pdfBitmap);
            pdfBitmap. Save(stream, PdfLibCore.Enums.BitmapImageFormat.Png);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            CurrentPageImage = bitmap;
            PageInfo = $"Page {_currentPageIndex + 1} of {_pdfDocument.Pages.Count}";
        }

        private void NextPage()
        {
            if (_pdfDocument != null && _currentPageIndex < _pdfDocument.Pages.Count - 1)
            {
                LoadPage(_currentPageIndex + 1);
            }
        }

        private void PreviousPage()
        {
            if (_pdfDocument != null && _currentPageIndex > 0)
            {
                LoadPage(_currentPageIndex - 1);
            }
        }

        public void HandleKeyDown(Key key)
        {
            switch (key)
            {
                case Key.Right:
                case Key.Down:
                    NextPage();
                    break;
                case Key.Left:
                case Key.Up:
                    PreviousPage();
                    break;
            }
        }
    }
}