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
            return !string.IsNullOrEmpty(username) &&
                   Regex.IsMatch(username, @"^
                        (?![_0-9])               # Не начинается с _ или цифры
                        (?!.*[_]{2,})            # Нет двух и более _ подряд
                        (?!.*[_]$)               # Не заканчивается на _
                        [a-zA-Z0-9_]{5,40}       # Длина 5-40, разрешенные символы
            $", RegexOptions.IgnorePatternWhitespace);
        }
    }
}
