using Al.ReflectionHelpers.Tests.Attrubutes;
using Al.ReflectionHelpers.Tests.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Xunit;

namespace Al.ReflectionHelpers.Tests
{
    public class HasAttributeProperty
    {
        [Fact]
        public void HasAttribute_Has_True()
        {
            var result = AttributesHelper.HasAttribute<TestClass, EditableAttribute>(nameof(TestClass.Id));

            Assert.True(result);
        }
        [Fact]
        public void HasAttribute_Not_False()
        {
            var result = AttributesHelper.HasAttribute<TestClass, ClassDataAttribute>(nameof(TestClass.Id));

            Assert.False(result);
        }

        [Fact]
        public void HasAttributeHasParameter_Has_True()
        {
            var result = AttributesHelper.HasAttribute<TestClass, EditableAttribute, bool>(
                nameof(TestClass.Id),
                x => x.AllowEdit,
                true);

            Assert.True(result);
        }


        [Fact]
        public void HasAttributeHasParameter_HasAttributeNotParameter_False()
        {
            var result = AttributesHelper.HasAttribute<TestClass, EditableAttribute, bool>(
                nameof(TestClass.Id),
                x => x.AllowEdit,
                false);

            Assert.False(result);
        }
    }
}