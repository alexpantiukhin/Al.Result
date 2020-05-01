using Microsoft.Extensions.Logging;

using System;

namespace Al
{
    public class Result<T>
    {
        public T Model { get; set; }
        public bool Success { get; private set; } = true;
        public string UserMessage { get; private set; }
        public string AdminMessage { get; private set; }
        public int ErrorCode { get; private set; }

        private ILogger _logger;

        //public Result()
        //{

        //}

        public Result(ILogger logger)// : this()
        {
            _logger = logger;
        }

        public void AddError(string userMessage, string adminMessage, LogLevel logLevel, int errorCode = 0)
        {
            if (_logger != null)
            {
                _logger.Log(logLevel, "Error code: " + errorCode + ". Error message: " + (string.IsNullOrWhiteSpace(adminMessage) ? userMessage : adminMessage));
            }

            AddError(userMessage, adminMessage, errorCode);

        }

        public void AddError(string userMessage, string adminMessage = null, int errorCode = 0)
        {
            Success = false;
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            ErrorCode = errorCode;
        }

        public void AddError(Exception e, string userMessage, int errorCode = 0)
        {
            var message = "Ошибка: " + (e != null ? e.ToString() : "exception = null");

            if (_logger != null)
            {
                _logger.LogError(e, message);
            }

            AddError(userMessage, message, errorCode);
        }

        /// <summary>
        /// Добавляет сообщение об успешности, если до этого не произошло ошибки
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="adminMessage"></param>
        public void AddSuccess(string userMessage, string adminMessage = null)
        {
            if (Success)
            {
                UserMessage = userMessage;
                AdminMessage = adminMessage;
            }
        }

        /// <summary>
        /// Конвертирует в ошибку другого типа
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <returns></returns>
        public Result<TNew> ToError<TNew>()
        {
            var result = new Result<TNew>(_logger);
            result.AddError(UserMessage, AdminMessage);
            return result;
        }

        public override string ToString()
        {
            return Success.ToString();
        }

        //public static Result<T> GetErrorResult(Exception e, string userMessage, string adminMessage = null, ILogger logger = null)
        //{
        //    Result<T> result = new Result<T>();
        //    var message = (adminMessage == null ? "" : (adminMessage + ". ")) + "Ошибка:" + e?.Message;

        //    if (logger != null)
        //        logger.LogError(e, message);

        //    result.AddError(userMessage, message);

        //    return result;
        //}
    }
}
