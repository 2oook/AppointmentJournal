using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using AppointmentJournal.Utils;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal.Infrastructure
{
    public static class UserTypeHtmlHelper
    {
        // HtmlHelper для вывода списка для перечисления UserType
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
