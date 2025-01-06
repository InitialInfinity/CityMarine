using Master.API.Repository.Interface;
using Master.API.Repository;
using Context;
using Master.Repository;
using Master.Repository.Interface;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddControllers();
builder.Services.AddScoped<IParameterMasterRepository, ParameterMasterRepository>();
builder.Services.AddScoped<IParameterValueMasterRepository, ParameterValueMasterRepository>();
builder.Services.AddScoped<ICityMasterRepository, CityMasterRepository>();
builder.Services.AddScoped<ICountryMasterRepository, CountryMasterRepository>();
builder.Services.AddScoped<IStateMasterRepository, StateMasterRepository>();
builder.Services.AddScoped<ICurrencyMasterRepository,CurrencyMasterRepository>();
builder.Services.AddScoped<ICurrencyRateMasterRepository, CurrencyRateMasterRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDesignationMasterRepository, DesignationMasterRepository>();
builder.Services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();
builder.Services.AddScoped<ICompanyDetailsRepository, CompanyDetailsRepository>();
builder.Services.AddScoped<ISubscriptionMasterRepository, SubscriptionMasterRepository>();
builder.Services.AddScoped<IServerAllocation, ServerAllocationRepository>();
builder.Services.AddScoped<ILoginDetailsRepository, LoginDetailsRepository>();
builder.Services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
builder.Services.AddScoped<IUnitConfigurationRepository, UnitConfigurationRepository>();
builder.Services.AddScoped<IGetWebMenuRepository, GetWebMenuRepository>();
builder.Services.AddScoped<IEmployeeEmailMgmtRepository, EmployeeEmailMgmtRepository>();
builder.Services.AddScoped<IEmailRuleConfgRepository, EmailRuleConfgRepository>();
builder.Services.AddScoped<IEmailListRepository, EmailListRepository>();
builder.Services.AddScoped<ICustomerMasterRepository, CustomerMasterRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ISentEmailRepository, SentEmailRepository>();
builder.Services.AddScoped<ISentClientRepository, SentClientRepository>();
builder.Services.AddScoped<IInboxEmailRepository, InboxEmailRepository>();
builder.Services.AddScoped<IInboxClientRepository, InboxClientRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true; // make the session cookie essential
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
    options.IdleTimeout = TimeSpan.FromMinutes(60); // set session timeout in minutes
});
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
