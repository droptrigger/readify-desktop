using System.Windows.Input;

namespace Readify.ViewModel.Base
{
    public class AsyncRelayParameterizedCommand : ICommand
    {
        /// <summary>
        /// Действие, которое будет запускаться
        /// </summary>
        private Func<object?, Task> _action;
        
        /// <summary>
        /// Событие, которое вызывается, когда <see cref="CanExecute(object?)"/> изменяется
        /// </summary>
        public event EventHandler? CanExecuteChanged;


        public AsyncRelayParameterizedCommand(Func<object?, Task> action)
        {
            _action = action;
        }


        /// <summary>
        /// RelayCommand может выполняться всегда
        /// </summary>
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
            await _action(parameter);
        }
    }
}
