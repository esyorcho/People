using System;
using System.ComponentModel;
using System.Reflection;

namespace People.Client.Helpers
{
    public class Enums
    {
        public enum Gender
        {
            [Description("Male")]
            M,
            [Description("Female")]
            F,
            [Description("Transgender")]
            T,
            Y
        }
    }

    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}