using Microsoft.Extensions.Logging;

using System;

namespace Al
{
    public class Result
    {
        public bool Success { get; private set; } = true;
        public string UserMessage { get; private set; }
        public string AdminMessage { get; private set; }
        public int ErrorCode { get; private set; }

        protected ILogger _logger;

        //public Result()
        //{

        //}

        public Result(ILogger logger)// : this()
        {
            _logger = logger;
        }

        public Result AddError(string userMessage, string adminMessage, LogLevel logLevel, int errorCode = 0)
        {
            if (_logger != null)
            {
                _logger.Log(logLevel, "Error code: " + errorCode + ". Error message: " + (string.IsNullOrWhiteSpace(adminMessage) ? userMessage : adminMessage));
            }

            AddError(userMessage, adminMessage, errorCode);

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

            if (_logger != null)
            {
                _logger.LogError(e, message);
            }

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

        public Result<T> Convert<T>()
        {
            var result = new Result<T>(_logger);
            return result;
        }

        /// <summary>
        /// Конвертирует в ошибку другого типа
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <returns></returns>
        public Result<T> ToError<T>()
        {
            var result = new Result<T>(_logger);
            result.AddError(UserMessage, AdminMessage);
            return result;
        }

    }
}
