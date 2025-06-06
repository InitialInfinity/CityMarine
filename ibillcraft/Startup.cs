using ibillcraft.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using System.Text;

namespace ibillcraft
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IConfiguration>(Configuration);
			services.AddControllers();

          //  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.WithOrigins().AllowAnyOrigin()
											.AllowAnyHeader()
											.AllowAnyMethod();
					});
			});
			services.AddDistributedMemoryCache(); // Add a distributed cache for session data to work
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true; // make the session cookie essential
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
                options.IdleTimeout = TimeSpan.FromMinutes(60); // set session timeout in minutes
            });
            services.AddMvc();


			//RUSHIKA
			services.AddLocalization(options =>
			{
				options.ResourcesPath = "Resources";
			});

			services.Configure<RequestLocalizationOptions>(options =>
			{
				options.SetDefaultCulture("en-US");
				options.AddSupportedUICultures("en-US", "de-DE", "hi-IN", "ar-SA");
				options.FallBackToParentUICultures = true;
				options
				.RequestCultureProviders
				.Remove((IRequestCultureProvider)typeof(AcceptLanguageHeaderRequestCultureProvider));
			});
			services.AddRazorPages()
				.AddViewLocalization();
			services.AddScoped<RequestLocalizationCookiesMiddleware>();

			//
		}	
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//Rushika
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}
            app.UseStaticFiles();
            app.UseRequestLocalizationCookies();
			//

            app.UseHttpsRedirection();
			app.UseSession();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
		
	}
}
