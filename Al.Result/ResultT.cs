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


        public override string ToString()
        {
            return Success.ToString();
        }
    }
}
