using Readify.Pages;
using Readify.Services.Base;
using System.Windows.Controls;

namespace Readify.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _mainFrame;
        private Frame _menuFrame;

        /// <summary>
        /// Устанавливает главный фрейм приложения
        /// </summary>
        public void SetMainFrame(Frame frame) => _mainFrame = frame;

        /// <summary>
        /// Устанавливает фрейм для вложенной навигации в меню
        /// </summary>
        public void SetMenuFrame(Frame frame) => _menuFrame = frame;

        /// <summary>
        /// Выполняет переход на указанный тип страницы
        /// </summary>
        public void NavigateTo<T>(object parameter = null!) where T : Page
        {
            var page = Activator.CreateInstance<T>();

            if (page is MainMenuPage && _mainFrame != null)
                _mainFrame.Navigate(page);
            else if (_menuFrame != null)
                _menuFrame.Navigate(page, parameter);
        }

        /// <summary>
        /// Возвращается к предыдущей странице в истории навигации
        /// </summary>
        public void GoBack()
        {
            if (_menuFrame?.CanGoBack ?? false)
                _menuFrame.GoBack();
        }
    }
}
