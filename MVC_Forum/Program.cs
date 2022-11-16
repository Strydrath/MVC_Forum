using Microsoft.AspNetCore.Localization;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
builder.Services.AddMvc()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization(options
        => options.ResourcesPath = "resources");
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
});
builder.Services.Configure<CookiePolicyOptions>(
    options => {
        options.CheckConsentNeeded = context => false;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

var supportedCultures = new[] { new
        CultureInfo("pl-PL"),new
        CultureInfo("en-US")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pl-PL"),
    FallBackToParentUICultures = true,
    FallBackToParentCultures = true,
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "trasa",
    pattern: "User/{action}",
    defaults: new { controller = "UserController" });

app.MapControllerRoute(
    name: "login",
    pattern: "Login/{name}",
    defaults: new { controller = "HomeController", action = "Login" });

app.Run();