using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoReservation.Dal
{
    /// <summary>
    /// This class contains static elements for logging / configuration.
    /// 
    /// WARNING:
    /// Please DO NOT implement your database context classes like this in production code.
    /// Here, we use static implementations since Dependency Injection is not covered
    /// in this module.
    /// </summary>
    public abstract class AutoReservationContextBase
        : DbContext
    {
        protected static readonly ILoggerFactory Logger;
        protected static readonly IConfiguration Configuration;

        static AutoReservationContextBase()
        {
            Logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole().SetMinimumLevel(LogLevel.Information);
            });

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json", 
                    optional: true,
                    reloadOnChange: true
                )
                .Build();           
        }

    }
}
