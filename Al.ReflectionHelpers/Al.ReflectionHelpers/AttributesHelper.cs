using System.Diagnostics.CodeAnalysis;

namespace Al.ReflectionHelpers
{
    public static class AttributesHelper
    {
        public static bool HasAttribute<T, TAttribute>()
            where T : class
            where TAttribute : Attribute
        {
            var classType = typeof(T);
            var attributes = classType.GetCustomAttributes(typeof(TAttribute), true);

            if (attributes.Length > 0)
                return true;

            return false;
        }

        public static bool HasAttribute<T, TAttribute, TAttributeProp>([NotNull] Func<TAttribute, TAttributeProp> attributePropFunc, TAttributeProp? attributePropValue)
            where T : class
            where TAttribute : Attribute
        {
            if (!HasAttribute<T, TAttribute>())
                return false;

            var classType = typeof(T);
            var attributes = classType.GetCustomAttributes(typeof(TAttribute), true);

            foreach (TAttribute attribute in attributes)
            {
                TAttributeProp? propValue = attributePropFunc(attribute);

                if (propValue == null)
                {
                    if (attributePropValue == null)
                        return true;
                }
                else
                {
                    if (propValue.Equals(attributePropValue))
                        return true;
                }
            }

            return false;
        }

        public static bool HasAttribute<T, TAttribute>([NotNull] string propertyName)
            where T : class
            where TAttribute : Attribute
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var classType = typeof(T);

            var property = classType.GetProperty(propertyName);

            if (property == null)
                throw new ArgumentNullException($"Не существует свойства {propertyName} в типе {classType.FullName}", nameof(property));

            var attributes = property.GetCustomAttributes(typeof(TAttribute), true);

            if (attributes.Any())
                return true;

            return false;
        }
        public static bool HasAttribute<T, TAttribute, TAttributeProp>([NotNull] string propertyName, [NotNull] Func<TAttribute, TAttributeProp> attributePropFunc, TAttributeProp? attributePropValue)
            where T : class
            where TAttribute : Attribute
        {
            if (!HasAttribute<T, TAttribute>(propertyName))
                return false;

            var classType = typeof(T);

            var property = classType.GetProperty(propertyName);

            if (property == null)
                throw new ArgumentNullException($"Не существует свойства {propertyName} в типе {classType.FullName}", nameof(property));

            var attributes = property.GetCustomAttributes(typeof(TAttribute), true);

            foreach (TAttribute attribute in attributes)
            {
                TAttributeProp? propValue = attributePropFunc(attribute);

                if (propValue == null)
                {
                    if (attributePropValue == null)
                        return true;
                }
                else
                {
                    if (propValue.Equals(attributePropValue))
                        return true;
                }
            }

            return false;
        }
    }
}