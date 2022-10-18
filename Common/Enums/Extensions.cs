using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Extensions
{
    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static List<string> GetDisplayNamesList(this Enum enumeration)
        {
            var outList = new List<string>();
            var enumValues = Enum.GetValues(enumeration.GetType());

            foreach (var enumValue in enumValues)
            {
                outList.Add(
                    enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name
                );
            }

            return outList;
        }
    }
}