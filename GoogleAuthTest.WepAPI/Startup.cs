using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GoogleAuthTest.WepAPI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{			
			services.AddControllers();

			services.AddAuthentication( option =>
			                            {
				                            option.DefaultScheme = "APIScheme";
			                            })
			        .AddCookie("APIScheme")
			        .AddGoogle(googleOptions =>
			                   {
				                   googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
				                   googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
													 googleOptions.AuthorizationEndpoint = "/google-login";

													 googleOptions.Events = new OAuthEvents()
													                        {
														                        OnTicketReceived = ctx =>
														                                           {
															                                           var username =
																                                           ctx.Principal.FindFirstValue(ClaimTypes
																	                                                                        .NameIdentifier);

															                                           ctx.Response.Redirect("/Index");
															                                           return Task.CompletedTask;
														                                           }
													                        };
			                   });

			services.AddMvc()
			        .AddRazorPagesOptions(o => o.Conventions.AuthorizePage("/Index"));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}
}
