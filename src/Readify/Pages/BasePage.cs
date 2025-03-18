using Readify.ViewModel.Base;
using Readify.ViewModel.Base.Initializer;
using System.Windows.Controls;

namespace Readify.Pages
{
    /// <summary>
    /// Базовый класс для создания страниц, хранящий ViewModel
    /// </summary>
    public abstract class BasePage : UserControl
    {
        /// <summary>
        /// ViewModel конкретной страницы
        /// </summary>
        public object? ViewModel { get; set; }
    }

    /// <summary>
    /// Базовый класс для создания страниц
    /// </summary>
    /// <typeparam name="Page">Страница</typeparam>
    /// <typeparam name="VM">View Model конкретной страницы</typeparam>
    public class BasePage<Page, VM> : BasePage, IAsyncInitializer<Page>
        where Page : BasePage<Page, VM>, new()
        where VM : BaseViewModel<VM>, new()
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="Page"/> ассинхронным способом
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Page"/></returns>
        public virtual async Task<Page> InitializeAsync()
        {
            ViewModel = await IAsyncInitializer<VM>.CreateAsync();
            DataContext = ViewModel;
            return (Page)this;
        }
    }
}
