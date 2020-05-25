using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GoogleAuthTest.WepAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//TODO:  Change to logging to desired levels
			Log.Logger = new LoggerConfiguration()
			             .MinimumLevel.Debug()
			             .WriteTo.Logger(l =>
			                             {
				                             l.WriteTo.File("debugLogs.txt");
			                             })
			             .WriteTo.Console(LogEventLevel.Information)
			             .CreateLogger();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
				    .UseSerilog()
						.ConfigureWebHostDefaults(webBuilder =>
						{
							webBuilder.UseStartup<Startup>();
						});
	}
}
