using Microsoft.Extensions.Logging;

using System;

namespace Al
{
    public interface IResult
    {
        bool Success { get; }
        string UserMessage { get; }
        string AdminMessage { get; }
        int ErrorCode { get; }
        IResult AddError(string userMessage, string adminMessage, LogLevel logLevel, int errorCode = 0);
        IResult AddError(string userMessage, string adminMessage = null, int errorCode = 0);
        IResult AddError(Exception e, string userMessage, int errorCode = 0);
        IResult AddSuccess(string userMessage, string adminMessage = null);
        IResult<T> Convert<T>();
    }
}
