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

builder.Services.AddSession(options => { options.Cookie.HttpOnly = true; });
builder.Services.Configure<CookiePolicyOptions>(
    options => {
        options.CheckConsentNeeded = context => false;
        options.MinimumSameSitePolicy = SameSiteMode.None;

    });
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
var cultures = new[] { new
        CultureInfo("pl-PL"),new
        CultureInfo("en-US")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pl-PL"),
    FallBackToParentUICultures = true,
    FallBackToParentCultures = true,
    SupportedCultures = cultures,
    SupportedUICultures = cultures
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "trasa",
    pattern: "User/{action}",
    defaults: new { controller = "UserController"});

app.MapControllerRoute(
    name: "login",
    pattern: "Login/{name}",
    defaults: new { controller = "HomeController", action="Login" });

app.Run();
