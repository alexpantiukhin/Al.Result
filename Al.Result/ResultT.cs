using Microsoft.Extensions.Logging;

namespace Al
{
    public class Result<T> : Result
    {
        public Result(ILogger logger) : base(logger)
        {
        }

        public T Model { get; set; }

        public Result<T> AddModel(T model)
        {
            Model = model;
            return this;
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
    }
}
