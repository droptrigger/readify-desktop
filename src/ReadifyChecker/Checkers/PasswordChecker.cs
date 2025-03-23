namespace ReadifyChecker
{
    /// <summary>
    /// Класс проверки пароля
    /// Наследование от <see cref="BaseChecker{P}"/>
    /// </summary>
    public class PasswordChecker : BaseChecker<string>
    {
        /// <summary>
        /// Метод проверки пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>True - если проверка пройдена</returns>
        public override bool Check(string? password)
        {
            return (
                password != null &&
                password.Length >= 8 &&
                password.Length <= 50 &&
                password.Any(char.IsDigit) &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                !password.All(char.IsLetterOrDigit)
                );
        }
    }
}
