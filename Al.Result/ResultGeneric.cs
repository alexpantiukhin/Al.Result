using Microsoft.Extensions.Logging;

namespace Al
{
    public class Result<T> : Result
    {
        public T Model { get; private set; }
        

        public Result(ILogger logger = null)
        {
            _logger = logger;
        }

        public Result<TNew> Convert<TNew>()
        {
            var result = new Result<TNew>(_logger);

            if (Success)
                result.AddSuccess(UserMessage, AdminMessage);
            else
                result.AddError(UserMessage, AdminMessage, ErrorCode);

            return result;
        }


        public Result<T> AddModel(T model, string userMessage = null, string adminMessage = null)
        {
            UserMessage = userMessage;
            AdminMessage = adminMessage;
            Model = model;
            return this;
        }

    }

}
