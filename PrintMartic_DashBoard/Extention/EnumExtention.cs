using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
namespace PrintMartic_DashBoard.Extention
{
    public static class EnumExtention
    {
        public static string GetEnumMemberValue(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var info = type.GetField(enumValue.ToString());
            var attribute = info.GetCustomAttribute<EnumMemberAttribute>(false);
            return attribute?.Value ?? enumValue.ToString();
        }
    }
}
