using Readify.Services.Exceptions.Enums;

namespace Readify.DTO
{
    /// <summary>
    /// Сущность, отвечающая за хранения ответа от сервера
    /// </summary>
    public class ResponseDTO
    {
        /// <summary>
        /// Является ли вызов успешным
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Если вызов является неуспешным, то здесь будет хранится ошибка
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Тип исключения, если он есть
        /// </summary>
        public TypesOfExceptions? TypeOfException { get; set; }

        /// <summary>
        /// Данное свойство хранит тело ответа
        /// </summary>
        public object? Result { get; set; }
    }
}
