using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using AppointmentJournal.Utils;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal.Infrastructure
{
    /// <summary>
    /// HtmlHelper для вывода списка для перечисления UserType
    /// </summary>
    public static class UserTypeHtmlHelper
    {
        /// <summary>
        /// Метод реализующий вывод списка для перечисления UserType
        /// </summary>
        /// <param name="html">Объект реализующий IHtmlHelper</param>
        /// <returns></returns>
        public static List<SelectListItem> CreateUserTypeList(this IHtmlHelper html)
        {
            var selectedItemsList = new List<SelectListItem>();

            selectedItemsList.Add(new SelectListItem("", "", true));
            selectedItemsList.Add(new SelectListItem(UserType.Consumer.GetAttribute<DisplayAttribute>().Name, ((int)UserType.Consumer).ToString()));
            selectedItemsList.Add(new SelectListItem(UserType.Producer.GetAttribute<DisplayAttribute>().Name, ((int)UserType.Producer).ToString()));

            return selectedItemsList;
        }
    }
}
