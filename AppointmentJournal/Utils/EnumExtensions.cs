using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppointmentJournal.Utils
{
    /// <summary>
    /// Методы расширения для работы с перечислениями
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Метод для получения атрибута
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута</typeparam>
        /// <param name="enumValue">Значение перечисления</param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
        }
    }
}
