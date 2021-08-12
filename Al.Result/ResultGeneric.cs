using Microsoft.Extensions.Logging;

using System;

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

        private Result() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger"></param>
        public Result(ILogger logger = null) : base(logger) { }

        /// <summary>
        /// Добавляет модель к результату, даже если до этого была ошибка.
        /// Сообщения добавляются, если не было ошибки
        /// </summary>
        /// <param name="model">Модель, которую нужно вернуть</param>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="adminMessage">Сообщение администратору</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <returns></returns>
        public Result<T> AddModel(T model, string userMessage = null, string adminMessage = null, LogLevel? logLevel = null)
        {
            Model = model;

            if (Success)
                SetProps(userMessage, adminMessage, 0, logLevel, null);
            
            return this;
        }

        /// <summary>
        /// Записывает ошибку в результат и записывает в лог при наличии
        /// </summary>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="adminMessage">Сообщение администратору</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <returns></returns>
        public new Result<T> AddError(string userMessage, string adminMessage = null, int errorCode = 0, LogLevel? logLevel = null)
        {
            Success = false;
            SetProps(userMessage, adminMessage, errorCode, logLevel, null);
            return this;
        }

        /// <summary>
        /// Добавляет ошибку к результату и записывает в лог при наличии
        /// </summary>
        /// <param name="e">Ошибка</param>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <returns></returns>
        public new Result<T> AddError(Exception e, string userMessage, int errorCode = 0, LogLevel? logLevel = null)
        {
            Success = false;
            var adminMessage = GetAdminErrorMessage(e);
            SetProps(userMessage, adminMessage, errorCode, logLevel, e);
            return this;
        }

    }

}
