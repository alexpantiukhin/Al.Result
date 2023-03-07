using System;

namespace Al.ReflectionHelpers.Tests.Attrubutes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute
    {
        public bool Param { get; }
        TestClassAttribute() : base() { }
        public TestClassAttribute(bool param) : base()
        {
            Param = param;
        }
    }
}
