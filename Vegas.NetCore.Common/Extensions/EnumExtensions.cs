using System;
using System.ComponentModel;
using System.Linq;

namespace Vegas.NetCore.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string Description(this Enum enumValue)
        {
            var description = string.Empty;
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Any())
                {
                    var descriptionAttribute = attrs[0] as DescriptionAttribute;
                    description = descriptionAttribute.Description;
                }
            }
            return description;
        }

        public static TEnum Next<TEnum>(this TEnum enumeration) where TEnum : Enum
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type.FullName} is not an enum");
            }
            var values = (TEnum[])Enum.GetValues(type);
            int index = Array.IndexOf(values, enumeration) + 1;
            return values.Length == index ? values[0] : values[index];
        }
    }
}
