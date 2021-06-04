using Microsoft.Extensions.Logging;

using System;

namespace Al
{
    /// <summary>
    /// Универсальная модель результата любых действий, содержащая
    /// флаг успешности действия, сообщения пользователю и администратору,
    /// а также производящая запись в лог при необходимости
    /// </summary>
    public class Result
    {
        public bool Success { get; protected set; } = true;
        public string UserMessage { get; protected set; }
        public string AdminMessage { get; protected set; }
        public int ErrorCode { get; protected set; }

        protected ILogger _logger;


        public Result(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Записывает ошибку в результат и записывает в лог при наличии
        /// </summary>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="adminMessage">Сообщение администратору</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <returns></returns>
        public Result AddError(string userMessage, string adminMessage = null, int errorCode = 0, LogLevel? logLevel = null)
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
        /// <param name="adminMessage">Сообщение администратору</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <returns></returns>
        public Result AddError(Exception e, string userMessage, int errorCode = 0, LogLevel? logLevel = null)
        {
            Success = false;
            var adminMessage = GetAdminErrorMessage(e);
            SetProps(userMessage, adminMessage, errorCode, logLevel, e);
            return this;
        }

        protected void SetProps(string userMessage, string adminMessage, int errorCode, LogLevel? logLevel, Exception e)
        {
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            ErrorCode = errorCode;

            if (_logger != null && logLevel != null)
            {
                var message = string.IsNullOrWhiteSpace(AdminMessage) ? UserMessage : AdminMessage;

                if (e == null)
                    _logger.Log(logLevel.Value, message);
                else
                    _logger.Log(logLevel.Value, e, message);
            }
        }

        protected string GetAdminErrorMessage(Exception e)
        {
            return "Ошибка: " + (e != null ? e.ToString() : "exception = null");
        }

        /// <summary>
        /// Добавляет сообщение об успешности, если до этого не произошло ошибки
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        public Result AddSuccess(string userMessage, string adminMessage = null, LogLevel? logLevel = null)
        {
            if (Success)
                SetProps(userMessage, adminMessage, 0, logLevel, null);

            return this;
        }
        /// <summary>
        /// Преобразует модель результата в модель типизированную другим типом
        /// </summary>
        /// <typeparam name="TNew">Новый тип</typeparam>
        /// <returns></returns>
        public Result<TNew> Convert<TNew>()
        {
            var result = new Result<TNew>(_logger);

            if (Success)
                result.AddSuccess(UserMessage, AdminMessage);
            else
                result.AddError(UserMessage, AdminMessage, ErrorCode);

            return result;
        }

        public override string ToString()
        {
            return Success.ToString();
        }
    }
}
