using Ninject;
using Readify.Services.Connection;

namespace Readify.IoC
{
    /// <summary>
    /// Настройка для Ninject IoC
    /// </summary>
    public sealed class IoC
    {
        /// <summary>
        /// Является ленивым экземпляром класса <see cref="IoC"/>
        /// </summary>
        private static readonly Lazy<IoC> IoCInstance = new Lazy<IoC>(() =>
        {
            IoC instance = new IoC();
            instance.Start();
            return instance;
        });

        /// <summary>
        /// Свойство для получения экземпляра <see cref="IoC"/>
        /// </summary>
        public static IoC Instance
        {
            get => IoCInstance.Value;
        }

        /// <summary>
        /// Определяет ядро Inversion of Control
        /// </summary>
        public IKernel Kernel { get; set; } = new StandardKernel();

        /// <summary>
        /// Метод используется для получения зависимости из контекста Ninject
        /// </summary>
        /// <typeparam name="T">Тип обьекта, который хотим получить из контекста</typeparam>
        /// <returns>Полученный обьект из контекста</returns>
        public static T Get<T>()
        {
            return IoC.Instance.Kernel.Get<T>();
        }

        /// <summary>
        /// Метод используется для первоначальный настройки Ninject IoC
        /// </summary>
        public void Start()
        {
            BindViewModels();
            BindServices();
            BindCheckers();
        }

        /// <summary>
        /// Метод используется для первоначального биндинга всех необходимых ViewModel в контексте Ninject IoC
        /// </summary>
        private void BindViewModels()
        {
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }

        /// <summary>
        /// Метод используется для первоначального биндига всех сервисов
        /// </summary>
        private void BindServices()
        {
            Kernel.Bind<UserApiClient>().ToSelf().InSingletonScope();
            Kernel.Bind<IAuthenticationSerivce>().ToConstant(new AuthenticationService());
            Kernel.Bind<IUserService>().ToConstant(new UserService());
            Kernel.Bind<IMessageService>().ToConstant(new MessageService());
        }

        /// <summary>
        /// Метод используется для биндинга проверщиков
        /// </summary>
        private void BindCheckers()
        {
            Kernel.Bind<EmailChecker>().ToConstant(new EmailChecker());
            Kernel.Bind<PasswordChecker>().ToConstant(new PasswordChecker());
            Kernel.Bind<PhoneChecker>().ToConstant(new PhoneChecker());
            Kernel.Bind<LoginChecker>().ToConstant(new LoginChecker());
        }
    }
}
