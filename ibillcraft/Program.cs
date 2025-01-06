using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews()
     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<RequestLocalizationOptions>(Options =>
{
    //Options.SetDefaultCulture("en-US");
    //Options.AddSupportedCultures("en-US", "de-DE", "ar-SA", "hi-IN");
    //Options.FallBackToParentUICultures = true;
    var supportedculture = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
        new CultureInfo("ar-SA"),
        new CultureInfo("hi-IN"),
        new CultureInfo("kn-IN"),
        new CultureInfo("mr-IN"),
    };
    Options.DefaultRequestCulture = new RequestCulture("en-US");
    //Options.DefaultRequestCulture = new RequestCulture("");
    Options.SupportedUICultures = supportedculture;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true; // make the session cookie essential
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
    options.IdleTimeout = TimeSpan.FromMinutes(60); // set session timeout in minutes
});
var app = builder.Build();
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddConsole() // Use console logger for development
        .AddDebug();   // Use debug logger for development
});
var logger = loggerFactory.CreateLogger<Program>();
app.UseRequestLocalization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();
