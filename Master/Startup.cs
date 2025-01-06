using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ServiceProxy = Master.API.Repository.ServiceProxy;
using common;
using Context;
using Master.API.Repository.Interface;
using Master.API.Repository;
using DapperContext = Context.DapperContext;
using Master.Repository;
using Master.Repository.Interface;


namespace Master.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<DapperContext>();
            services.AddScoped<ICityMasterRepository, CityMasterRepository>();
            services.AddScoped<ICountryMasterRepository, CountryMasterRepository>();
            services.AddScoped<IStateMasterRepository, StateMasterRepository>();
            services.AddScoped<ICurrencyMasterRepository, CurrencyMasterRepository>();
            services.AddScoped<ICurrencyRateMasterRepository, CurrencyRateMasterRepository>();
            services.AddScoped<IParameterMasterRepository, ParameterMasterRepository>();
            services.AddScoped<IParameterValueMasterRepository, ParameterValueMasterRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();
            services.AddScoped<IDesignationMasterRepository, DesignationMasterRepository>();
            services.AddScoped<ICompanyDetailsRepository, CompanyDetailsRepository>();
            services.AddScoped<ISubscriptionMasterRepository, SubscriptionMasterRepository>();
            services.AddScoped<IServerAllocation, ServerAllocationRepository>();
            services.AddScoped<ILoginDetailsRepository, LoginDetailsRepository>();
            services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
            services.AddScoped<IUnitConfigurationRepository, UnitConfigurationRepository>();
            services.AddScoped<IGetWebMenuRepository, GetWebMenuRepository>();
            services.AddScoped<IEmployeeEmailMgmtRepository, EmployeeEmailMgmtRepository>();
            services.AddScoped<IEmailRuleConfgRepository, EmailRuleConfgRepository>();
            services.AddScoped<IEmailListRepository, EmailListRepository>();
            services.AddScoped<ICustomerMasterRepository, CustomerMasterRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<ISentEmailRepository, SentEmailRepository>();
            services.AddScoped<ISentClientRepository, SentClientRepository>();
            services.AddScoped<IInboxEmailRepository, InboxEmailRepository>();
            services.AddScoped<IInboxClientRepository, InboxClientRepository>();
            services.AddControllers();
            services.AddMvc(options =>
            {
                //an instant  
                options.Filters.Add<ExampleFilterAttribute>();
                //By the type  
                options.Filters.Add(typeof(ExampleFilterAttribute));
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<ExampleFilterAttribute>();
            });
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
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true; // make the session cookie essential
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
                options.IdleTimeout = TimeSpan.FromMinutes(60); // set session timeout in minutes
            });
            //var serviceProvider = services.BuildServiceProvider();
            //var dapperContext = serviceProvider.GetRequiredService<DapperContext>();
            //ServiceProxy.Configure(dapperContext);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
