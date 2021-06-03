using Microsoft.Extensions.Logging;

using System;

namespace Al
{
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
        /// Записывает ошибку в результат и в лог при наличии
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        /// <param name="logLevel"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public Result AddError(string userMessage, string adminMessage, LogLevel logLevel, int errorCode = 0)
        {
            AddError(userMessage, adminMessage, errorCode);

            SendLog(logLevel, userMessage, adminMessage, errorCode);

            return this;
        }

        public Result AddError(string userMessage, string adminMessage = null, int errorCode = 0)
        {
            Success = false;
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            ErrorCode = errorCode;

            return this;
        }

        public Result AddError(Exception e, string userMessage, int errorCode = 0)
        {
            var message = "Ошибка: " + (e != null ? e.ToString() : "exception = null");

            AddError(userMessage, message, errorCode);

            return this;
        }

        /// <summary>
        /// Добавляет ошибку к результату и записывает в лог при наличии
        /// </summary>
        /// <param name="e"></param>
        /// <param name="userMessage"></param>
        /// <param name="logLevel"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public Result AddError(Exception e, string userMessage, LogLevel logLevel, int errorCode = 0)
        {
            var message = "Ошибка: " + (e != null ? e.ToString() : "exception = null");

            SendLog(logLevel, userMessage, message, errorCode, e);

            AddError(userMessage, message, errorCode);

            return this;
        }

        /// <summary>
        /// Добавляет сообщение об успешности, если до этого не произошло ошибки
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        public Result AddSuccess(string userMessage, string adminMessage = null)
        {
            if (Success)
            {
                UserMessage = userMessage;
                AdminMessage = adminMessage;
            }

            return this;
        }

        public override string ToString()
        {
            return Success.ToString();
        }

        //public Result<TNew> Convert<TNew>()
        //{
        //    var result = new Result<TNew>(_logger);

        //    if (Success)
        //        result.AddSuccess(UserMessage, AdminMessage);
        //    else
        //        result.AddError(UserMessage, AdminMessage, ErrorCode);

        //    return result;
        //}


        //public Result<T> AddModel(T model, string userMessage = null, string adminMessage = null)
        //{
        //    UserMessage = userMessage;
        //    AdminMessage = adminMessage;
        //    Model = model;
        //    return this;
        //}

        protected void SendLog(LogLevel logLevel, string userMessage, string adminMessage, int errorCode, Exception e = null)
        {
            if (_logger != null)
            {
                var message = $"Error code: {errorCode}. Error message: {(string.IsNullOrWhiteSpace(adminMessage) ? userMessage : adminMessage)}";

                if (e == null)
                    _logger.Log(logLevel, e, message);
                else
                    _logger.Log(logLevel, message);
            }
        }
    }
}
