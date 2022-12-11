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
        /// <summary>
        /// Статус результата
        /// </summary>
        public bool Success { get; protected set; } = true;
        /// <summary>
        /// Сообщение пользователям
        /// </summary>
        public string UserMessage { get; protected set; }
        /// <summary>
        /// Сообщение администраторам
        /// </summary>
        public string AdminMessage { get; protected set; }
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; protected set; }
        /// <summary>
        /// Логгер
        /// </summary>
        private ILogger _logger;

        LogLevel? LogLevel;

        private Result() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger"></param>
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
        /// <param name="writeLog">необходимо ли записать в лог</param>
        /// <returns></returns>
        public Result AddError(
            string userMessage,
            string adminMessage = null,
            int errorCode = 0,
            LogLevel? logLevel = null,
            bool writeLog = true)
        {
            Success = false;
            SetProps(userMessage, adminMessage, errorCode, logLevel, null, writeLog);
            return this;
        }


        /// <summary>
        /// Добавляет ошибку к результату и записывает в лог при наличии
        /// </summary>
        /// <param name="e">Ошибка</param>
        /// <param name="userMessage">Сообщение пользователю</param>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="logLevel">Уровень логгирования. Если передан, то ошибка записывается в лог</param>
        /// <param name="writeLog">необходимо ли записать в лог</param>
        /// <returns></returns>
        public Result AddError(Exception e,
            string userMessage,
            int errorCode = 0,
            LogLevel? logLevel = null,
            bool writeLog = true)
        {
            Success = false;
            var adminMessage = GetAdminErrorMessage(e);
            SetProps(userMessage, adminMessage, errorCode, logLevel, e, writeLog);
            return this;
        }

        /// <summary>
        /// Устанавливает свойства
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="logLevel"></param>
        /// <param name="e"></param>
        /// <param name="writeLog">необходимо ли записать в лог</param>
        protected void SetProps(string userMessage,
            string adminMessage,
            int errorCode,
            LogLevel? logLevel,
            Exception e,
            bool writeLog = true)
        {
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            ErrorCode = errorCode;
            LogLevel = logLevel;

            if (writeLog && _logger != null && logLevel != null)
            {
                var message = string.IsNullOrWhiteSpace(AdminMessage) ? UserMessage : AdminMessage;

                if (e == null)
                    _logger.Log(logLevel.Value, message);
                else
                    _logger.Log(logLevel.Value, e, message);
            }
        }

        /// <summary>
        /// Создаёт сообение об ошибке 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected string GetAdminErrorMessage(Exception e)
        {
            return "Ошибка: " + (e != null ? e.ToString() : "exception = null");
        }

        /// <summary>
        /// Добавляет сообщение об успешности, если до этого не произошло ошибки
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        /// <param name="logLevel"></param>
        /// <param name="writeLog">необходимо ли записать в лог</param>
        public Result AddSuccess(string userMessage, 
            string adminMessage = null,
            LogLevel? logLevel = null,
            bool writeLog = false)
        {
            if (Success)
                SetProps(userMessage, adminMessage, 0, logLevel, null, writeLog);

            return this;
        }

        /// <summary>
        /// Преобразует модель результата в модель типизированную другим типом
        /// </summary>
        /// <typeparam name="TNew">Новый тип</typeparam>
        /// <returns>Результат, типизированный новым типом</returns>
        public Result<TNew> Convert<TNew>()
        {
            var result = new Result<TNew>(_logger);

            if (Success)
                result.AddSuccess(UserMessage, AdminMessage, LogLevel, false);
            else
                result.AddError(UserMessage, AdminMessage, ErrorCode, LogLevel, false);

            return result;
        }

        /// <summary>
        /// Преобразует модель в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Success.ToString();
        }
    }
}
