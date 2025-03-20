using System.Windows.Input;

namespace Readify.ViewModel.Base
{
    public class AsyncRelayCommand : ICommand
    {
        /// <summary>
        /// Действие, которое будет запускаться
        /// </summary>
        private Func<Task> _action;

        /// <summary>
        /// Событие, которое вызывается когда <see cref="CanExecute(object)"/> изменяется
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AsyncRelayCommand(Func<Task> action)
        {
            _action = action;
        }

        /// <summary>
        /// RelayCommand может всегда выполняться
        /// </summary>
        /// <param name="parameter">Параметры для определения выполнения команды</param>
        /// <returns>Признак выполнения команды</returns>
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполняет команду Action
        /// </summary>
        /// <param name="parameter">Параметры для выполнения команды</param>
        public async void Execute(object? parameter)
        {
            await _action();
        }
    }
}
