using System.Collections.Generic;

namespace Xutils.Extensions
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var descriptionAttributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Any())
                return descriptionAttributes.FirstOrDefault()?.Description;

            var displayAttributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            return displayAttributes.FirstOrDefault()?.GetDescription() ?? value.ToString();
        }

        public static string GetDisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DisplayNameAttribute[] customAttributes = (DisplayNameAttribute[])field.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (customAttributes.Any())
                return customAttributes.First().DisplayName;
            return ((IEnumerable<DisplayAttribute>)field.GetCustomAttributes(typeof(DisplayAttribute), false)).FirstOrDefault()?.GetName() ?? value.ToString();
        }
    }
}
