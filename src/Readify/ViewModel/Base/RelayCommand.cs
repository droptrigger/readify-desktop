using System.Windows.Input;

namespace Readify.ViewModel.Base
{
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Действие, которое будет запускаться
        /// </summary>
        private Action _action;

        /// <summary>
        /// Событие, которое вызывается, когда <see cref="CanExecute(object?)"/> изменяется
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RelayCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// RelayCommand может выполняться всегда
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
        public void Execute(object? parameter) 
        {
            _action();
        }
    }
}
