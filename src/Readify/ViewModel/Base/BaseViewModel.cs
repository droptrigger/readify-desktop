using Readify.ViewModel.Base.Initializer;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Readify.ViewModel.Base
{
    /// <summary>
    /// Базовая View Model
    /// </summary>
    public abstract class BaseViewModel<Class> : IAsyncInitializer<Class>
        where Class : BaseViewModel<Class>, new()
    {
        /// <summary>
        /// Хранит в себе значения всех свойств конкретной реализации <see cref="BaseViewModel{Class}"/>
        /// </summary>
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        /// <summary>
        /// Вызывается при любом изменении состояния элемента на пользовательском интерфейсе
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Используется для получения свойства из словаря свойств <see cref="_propertyValues"
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="propertyName">Название свойства</param>
        /// <returns>Значение свойства</returns>
        protected T? Get<T>([CallerMemberName] string propertyName = null!)
        {
            if (_propertyValues.TryGetValue(propertyName, out object? value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Используеться для добавления свойства в словарь свойств <see cref="_propertyValues"/>
        /// </summary>
        /// <typeparam name="T">Тип значения свойства</typeparam>
        /// <param name="value">Значение свойства</param>
        /// <param name="propertyName">Название свойства</param>
        protected void Set<T>(T value, [CallerMemberName] string propertyName = null!)
        {
            if (!EqualityComparer<T>.Default.Equals(Get<T>(propertyName), value))
            {
                _propertyValues[propertyName] = value;
                OnPropertryChanged(propertyName);
            }
        }

        /// <summary>
        /// Вызывается для вызова <see cref="PropertyChanged"/> события
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertryChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Запускает ассинхронную команду
        /// </summary>
        /// <param name="flag">Булевое поле, определяющее запущена ли сейчас команда или нет</param>
        /// <param name="action">Действие, которое необходимо запустить</param>
        public async Task RunCommand(Expression<Func<bool>> flagExpression, Func<Task> action)
        {
            var flag = (PropertyInfo)((MemberExpression)flagExpression.Body).Member;
            if ((bool)flag.GetValue(this))
            {
                return;
            }

            flag.SetValue(this, true);

            try
            {
                await action();
            }
            finally
            {
                flag.SetValue(this, false);
            }
        }

        /// <summary>
        /// Используется для ассинхронной инициализации экземпляра текущего класса
        /// </summary>
        /// <returns>Экземпляр текущего класса</returns>
        public virtual Task<Class> InitializeAsync()
        {
            return Task.FromResult((Class)this);
        }

        /// <summary>
        /// Используется для вывода пользователю сообщения об ошибке
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(
                message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
        }
    }
}
