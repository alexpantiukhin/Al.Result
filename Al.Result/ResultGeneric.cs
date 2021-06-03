using Microsoft.Extensions.Logging;

namespace Al
{
    /// <summary>
    /// Универсальная типизированная модель результата любых действий, содержащая
    /// флаг успешности действия, сообщения пользователю и администратору,
    /// а также производящая запись в лог при необходимости
    /// </summary>
    public class Result<T> : Result
    {
        /// <summary>
        /// Модель результата
        /// </summary>
        public T Model { get; private set; }

        public Result(ILogger logger = null) : base(logger) { }

        /// <summary>
        /// Добавляет модель к результату, даже если до этого была ошибка.
        /// Сообщения добавляются, если не было ошибки
        /// </summary>
        /// <param name="model">Модель, которую нужно вернуть</param>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="adminMessage">Сообщение администратору</param>
        /// <returns></returns>
        public Result<T> AddModel(T model, string userMessage = null, string adminMessage = null)
        {
            Model = model;

            if (Success)
            {
                UserMessage = userMessage;
                AdminMessage = adminMessage;
            }
            
            return this;
        }

    }

}
