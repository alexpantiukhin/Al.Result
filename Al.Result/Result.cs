using Microsoft.Extensions.Logging;

using System;

namespace Al
{
    public class Result<T>
    {
        public bool Success { get; private set; } = true;
        public string UserMessage { get; private set; }
        public string AdminMessage { get; private set; }
        public int ErrorCode { get; private set; }

        protected ILogger _logger;


        public Result(ILogger logger = null)
        {
            _logger = logger;
        }

        public Result<T> AddError(string userMessage, string adminMessage, LogLevel logLevel, int errorCode = 0)
        {
            AddError(userMessage, adminMessage, errorCode);

            SendLog(logLevel, userMessage, adminMessage, errorCode);

            return this;
        }

        private void SendLog(LogLevel logLevel, string userMessage, string adminMessage, int errorCode, Exception e = null)
        {
            if (_logger != null)
            {
                var message = "Error code: " + errorCode + ". Error message: " + (string.IsNullOrWhiteSpace(adminMessage) ? userMessage : adminMessage);

                if (e == null)
                    _logger.Log(logLevel, e, message);
                else
                    _logger.Log(logLevel, message);
            }
        }

        public Result<T> AddError(string userMessage, string adminMessage = null, int errorCode = 0)
        {
            Success = false;
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            ErrorCode = errorCode;

            return this;
        }

        public Result<T> AddError(Exception e, string userMessage, int errorCode = 0)
        {
            var message = "Ошибка: " + (e != null ? e.ToString() : "exception = null");

            AddError(userMessage, message, errorCode);

            return this;
        }

        public Result<T> AddError(Exception e, string userMessage, LogLevel logLevel, int errorCode = 0)
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
        public Result<T> AddSuccess(string userMessage, string adminMessage = null)
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

            if (Success)
                AddSuccess(UserMessage, AdminMessage);
            else
                AddError(UserMessage, AdminMessage, ErrorCode);

            return result;
        }

        public T Model { get; set; }

        public Result<T> AddModel(T model)
        {
            Model = model;
            return this;
        }
    }
}
