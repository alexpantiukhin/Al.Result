using Al.ReflectionHelpers.Tests.Attrubutes;

using System.ComponentModel.DataAnnotations;

namespace Al.ReflectionHelpers.Tests.Models
{
    [TestClass(true)]
    public class TestClass
    {
        [Editable(true)]
        public int Id { get; set; }
    }
}
