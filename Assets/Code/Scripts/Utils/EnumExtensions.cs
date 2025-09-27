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
    }
}
