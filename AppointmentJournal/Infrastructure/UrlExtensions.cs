using Microsoft.AspNetCore.Http;

namespace AppointmentJournal.Infrastructure
{
    /// <summary>
    /// Методы расширения для работы с URL
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Метод для получения строки запроса
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <returns>Строка запроса</returns>
        public static string PathAndQuery(this HttpRequest request) => request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();
    }
}
