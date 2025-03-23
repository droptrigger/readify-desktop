namespace ReadifyChecker
{
    /// <summary>
    /// Базовый класс для реализации проверок
    /// </summary>
    public abstract class BaseChecker<P>
    {
        /// <summary>
        /// Используется для реализации проверок
        /// </summary>
        /// <param name="parameter">Параметр</param>
        /// <returns>True - если проверка пройдена</returns>
        public abstract bool Check(P parameter);
    }

}
