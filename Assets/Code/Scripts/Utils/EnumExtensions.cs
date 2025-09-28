using System;
using System.Text;

namespace Code.Scripts.Utils
{
    public static class EnumExtensions
    {
        public static string AddSpacesToEnum(this string enumString)
        {
            if (string.IsNullOrEmpty(enumString))
                return enumString;

            StringBuilder result = new StringBuilder();
            result.Append(enumString[0]);

            for (int i = 1; i < enumString.Length; i++)
            {
                if (char.IsUpper(enumString[i]))
                    result.Append(' ');

                result.Append(enumString[i]);
            }

            return result.ToString();
        }

        public static bool HasFlag(this System.Enum variable, System.Enum flag)
        {
            if (variable.GetType() != flag.GetType())
                throw new ArgumentException("Error: Unable to perform comparison. Enumeration types doesn't match.");

            int num = Convert.ToInt32(variable);
            int flagNum = Convert.ToInt32(flag);

            return (num & flagNum) == flagNum;
        }
    }
}
