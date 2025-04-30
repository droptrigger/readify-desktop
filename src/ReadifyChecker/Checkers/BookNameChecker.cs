using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadifyChecker.Checkers
{
    public class BookNameChecker : BaseChecker<string>
    {
        /// <summary>
        /// Метод проверки имени пользователя
        /// </summary>
        /// <param name="bookname">Имя пользователя</param>
        /// <returns>True - если проверка пройдена</returns>
        public override bool Check(string? bookname)
        {
            return !string.IsNullOrEmpty(bookname) &&
                   Regex.IsMatch(bookname, @"^
                        (?!.*[ ]{2,})            # Нет двух и более _ подряд
                        [a-zA-Zа-яА-Я0-9 ]{2,200}      # Длина 2-200, разрешенные символы
            $", RegexOptions.IgnorePatternWhitespace);
        }
    }
}
