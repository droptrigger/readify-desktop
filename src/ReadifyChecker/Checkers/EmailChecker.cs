using System.Text.RegularExpressions;

namespace ReadifyChecker
{
    /// <summary>
    /// Класс проверки почты
    /// Наследование от <see cref="BaseChecker{P}"/>
    /// </summary>
    public class EmailChecker : BaseChecker<string>
    {
        /// <summary>
        /// Используется для проверки почты
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>True - если проверка пройдена</returns>
        public override bool Check(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            Match match = regex.Match(email);

            return match.Success;
        }
    }
}
