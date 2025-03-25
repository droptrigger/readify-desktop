using System.Windows.Controls;

namespace Readify.Services.Base
{
    /// <summary>
    /// Интерфейс сервиса навигации для управления переходами между страницами
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Переход на указанную страницу
        /// </summary>
        /// <typeparam name="T">Тип страницы для навигации (наследование от <see cref="Page"/>)</typeparam>
        /// <param name="parameter">Дополнительные параметры</param>
        void NavigateTo<T>(object parameter = null!) where T : Page;

        /// <summary>
        /// Возврат к предыдущей странице в истории навигации
        /// </summary>
        void GoBack();

        /// <summary>
        /// Установка главного фрейма приложения
        /// </summary>
        /// <param name="frame">Ссылка на главный фрейм</param>
        void SetMainFrame(Frame frame);

        /// <summary>
        /// Установка фрейма меню для вложенной навигации
        /// </summary>
        /// <param name="frame">Ссылка на фрейм меню</param>
        void SetMenuFrame(Frame frame);
    }
}
