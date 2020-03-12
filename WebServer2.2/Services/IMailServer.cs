using System;
using System.Net;

namespace WebServer2._2.Services
{
    public interface IMailServer : IDisposable
    {
        /// <summary>
        /// Работает ли данный сервер
        /// </summary>
        bool IsListening { get; set; }

        /// <summary>
        /// Обработка запроса. Может вызываться в другом потоке!
        /// </summary>
        /// <param name="context"></param>
        void Handle(HttpListenerContext context);
    }
}