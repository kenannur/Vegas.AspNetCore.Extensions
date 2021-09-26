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
    }
}
