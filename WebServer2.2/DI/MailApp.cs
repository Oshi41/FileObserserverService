using ConsoleDI.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebServer2._2.Services;

namespace WebServer2._2.DI
{
    public class MailApp : DependencyInjectionBase
    {
        public MailApp()
            : base(new ServiceCollection(), new ConfigurationBuilder())
        {
        }

        protected override void Init(IServiceCollection collection, IConfigurationBuilder builder)
        {
            builder.AddJsonFile("settings.json");

#if DEBUG
            collection.AddTransient<IMailServer, TestWebServer>();
#else
            collection.AddTransient<IMailServer, WebServer>();
#endif
        }

        protected override void ApplySettings(IServiceCollection collection, IConfigurationRoot settings)
        {
        }
    }
}