using System.Text.RegularExpressions;

namespace ReadifyChecker
{
    /// <summary>
    /// Класс имени пользователя
    /// Наследование от <see cref="BaseChecker{P}"/>
    /// </summary>
    public class UsernameChecker : BaseChecker<string>
    {
        /// <summary>
        /// Метод проверки имени пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>True - если проверка пройдена</returns>
        public override bool Check(string? username)
        {
            return (
                !string.IsNullOrEmpty(username) &&
                Regex.IsMatch(username, "^[a-zA-Z0-9]+$") &&
                username.All(char.IsLetterOrDigit) &&
                username.Length >= 5 &&
                username.Length <= 40
                );
        }
    }
}
