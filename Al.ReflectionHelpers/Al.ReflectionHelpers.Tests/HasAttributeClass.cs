using Al.ReflectionHelpers.Tests.Attrubutes;
using Al.ReflectionHelpers.Tests.Models;

using System.Collections.Generic;

using Xunit;

namespace Al.ReflectionHelpers.Tests
{
    public class HasAttributeClass
    {
        [Fact]
        public void HasAttribute_Has_True()
        {
            var result = AttributesHelper.HasAttribute<TestClass, TestClassAttribute>();

            Assert.True(result);
        }

        [Fact]
        public void HasAttribute_NotHas_False()
        {
            var result = AttributesHelper.HasAttribute<TestClass, TestClass2Attribute>();

            Assert.False(result);
        }

        [Fact]
        public void HasAttributeHasParameter_Has_True()
        {
            var result = AttributesHelper.HasAttribute<TestClass, TestClassAttribute, bool>(
                x => x.Param, true);

            Assert.True(result);
        }

        [Fact]
        public void HasAttributeHasParameter_HasAttributeNotParameter_True()
        {
            var result = AttributesHelper.HasAttribute<TestClass, TestClassAttribute, bool>(
                x => x.Param, false);

            Assert.False(result);
        }
    }
}