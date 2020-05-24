using System;
using System.Collections.Generic;
using System.Text;

namespace Al
{
    public interface IResult<T>
    {
        T Model { get; set; }
        IResult<T> AddModel(T model);
    }
}
