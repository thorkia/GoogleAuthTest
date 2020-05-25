using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
				                   googleOptions.SignInScheme = "APIScheme";
				                   googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
				                   googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];

				                   //googleOptions.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

													 googleOptions.Events = new OAuthEvents()
													                        {
														                        OnTicketReceived = ctx =>
														                                           {
															                                           var username =
																                                           ctx.Principal.FindFirstValue(ClaimTypes
																	                                                                        .NameIdentifier);
																																				 //TODO: Add user check/creation here 

															                                           ctx.Response.Redirect("/Index");
																																				 ctx.HandleResponse();
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
