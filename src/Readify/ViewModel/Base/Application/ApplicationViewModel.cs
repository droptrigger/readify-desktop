using Readify.Pages;

namespace Readify.ViewModel.Base.Application
{
    /// <summary>
    /// Наследуется от <see cref="BaseViewModel{Class}"/>
    /// Является ViewModel целого приложения. Глобальный ViewModel.
    /// </summary>
    public class ApplicationViewModel : BaseViewModel<ApplicationViewModel>
    {
        /// <summary>
        /// Свойство, используемое для выполнения действия при изменении страницы
        /// </summary>
        public event EventHandler? CurrentPagePropertyChanged;

        public BasePage? CurrentPage
        {
            get => Get<BasePage>();
            private set => Set(value);
        }

        // TODO: DTO user

        /// <summary>
        /// Метод, использующийся для переключения страницы
        /// </summary>
        /// <param name="basePage">Класс, хранящий ViewModel</param>
        public void GoToPage(BasePage basePage)
        {
            CurrentPage = basePage;
            OnPropertryChanged();
        }

        /// <summary>
        /// Используется для вызова события текущей страницы для указания,
        /// что элементы пользовательского интерфейса поменялись
        /// </summary>
        private void OnPropertryChanged() 
        { 
            CurrentPagePropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
