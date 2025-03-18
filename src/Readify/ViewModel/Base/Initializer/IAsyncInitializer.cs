

using System.Windows;

namespace Readify.ViewModel.Base.Initializer
{
    public interface IAsyncInitializer<Class>
        where Class : IAsyncInitializer<Class>, new()
    {
        /// <summary>
        /// Используется для ассинхронного создания экземпляра класса <see cref="Class"/>
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Class"/></returns>
        public static async Task<Class> CreateAsync()
        {
            return await Application.Current.Dispatcher.Invoke(async () =>
            {
                var instance = new Class();
                return await instance.InitializeAsync();
            });
        }

        /// <summary>
        /// Используется для инициализации экземпляра класса <see cref="Class"/>
        /// </summary>
        /// <returns>Экземпляр класса <see cref="Class"/></returns>
        Task<Class> InitializeAsync();
    }
}
