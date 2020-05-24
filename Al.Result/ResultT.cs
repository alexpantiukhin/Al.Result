using Microsoft.Extensions.Logging;

namespace Al
{
    public class Result<T> : Result, IResult<T>
    {
        public Result(ILogger logger) : base(logger)
        {
        }

        public T Model { get; set; }

        public IResult<T> AddModel(T model)
        {
            Model = model;
            return this;
        }
    }
}
