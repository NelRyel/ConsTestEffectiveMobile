using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsTestEffectiveMobile
{
    public static class MyLog
    {
        public static void AddLogging(this IServiceCollection services, Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(logLevel);
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }

    }
}
