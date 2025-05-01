using Readify.DTO.Books;
using Readify.DTO.Library;
using Readify.ViewModels.Base;

namespace Readify.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private BookDTO _book = null!;

        /// <summary>
        /// Текущая книга
        /// </summary>
        public BookDTO Book
        {
            get => _book;
            set => SetField(ref _book, value); 
        }

        /// <summary>
        /// Последняя прочитанная книга пользователя
        /// </summary>
        public SeeLibraryBookDTO? LastBook
        {
            get => App.CurrentUserLibrary.LastBook;
        } 

        /// <summary>
        /// Видимость последней прочитанной книги
        /// </summary>
        public bool IsLastBookVisible
        {
            get => LastBook is not null;
        }

        /// <summary>
        /// Видимость кнопки "Удалить из библиотеки"
        /// </summary>
        public bool IsBtnDeleteFromLibraryVisible
        {
            get
            {
                return App.CurrentUser.Books?.Any(lib =>
                    lib?.Book != null &&
                    lib.Book.Id == Book.Id)
                    ?? false;
            }
        }

        /// <summary>
        /// Видимость кнопки "Добавить в библиотеки"
        /// </summary>
        public bool IsBtnAddToLibraryVisible
        {
            get => !IsBtnDeleteFromLibraryVisible;
        }

        /// <summary>
        /// Видимость кнопки "Написать отзыв"
        /// </summary>
        public bool IsBtnWriteReviewVisible
        {
            get => !IsBtnDeleteReviewVisible;
        }

        /// <summary>
        /// Видимость кнопки "Удалить отзыв"
        /// </summary>
        public bool IsBtnDeleteReviewVisible
        {
            get
            {
                return App.CurrentUser.Reviews?.Any(r =>
                    r?.Book != null &&
                    r.Book.Id == Book.Id)
                    ?? false;
            }
        }

        /// <summary>
        /// Видимость кнопки "Удалить с сервиса"
        /// </summary>
        public bool IsBtnDeleteFromServiceVisible
        {
            get 
            {
                return App.CurrentUser.IdRole == 2 || App.CurrentUser.Id == Book.Author!.Id;
            }
        }

        /// <summary>
        /// Текст кто опубликовал книгу
        /// </summary>
        public string DeployerText
        {
            get => $"Опубликовал: {Book.Author!.Nickname}";
        }

        /// <summary>
        /// Текст с жанром
        /// </summary>
        public string GenreText
        {
            get => $"Жанр: {Book.Genre!.Name}";
        }

        public string RatingText
        {
            get => $"{GetRatingBook(Book)}/5";
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="book"></param>
        public BookViewModel(BookDTO book)
        {
            Book = book;
        }



        /// <summary>
        /// Метод получения рейтинга книги
        /// </summary>
        /// <returns></returns>
        private double GetRatingBook(BookDTO book)
        {
            var ratings =  book.Reviews?.Select(r => r.Rating).OfType<byte>().ToList();
            if (ratings!.Count == 0)
                return 0.0;

            double average = ratings.Average(r => r);
            return average;
        }
    }
}
