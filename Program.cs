using CommonLibrary.AuthPack;
using DxBlazorApplication7;
using DxBlazorApplication7.Data;
using DxBlazorApplication7.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

var webApOpts = new WebApplicationOptions
{
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ?
        AppContext.BaseDirectory : default,
    Args = args
};
var builder = WebApplication.CreateBuilder(webApOpts);
builder.Host.UseWindowsService();

builder.Services.AddDbContext<DSDBContext>
    (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

//builder.Services.AddScoped<DataService>();
builder.Services.AddHostedService<DataTimeService>();
builder.Services.AddTransient<DataLogService>();
builder.Services.AddScoped<AuthorizeUserService, AuthorizeService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

#region �[�J�ϥ� Cookie �{�һݭn���ŧi
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.CheckConsentNeeded = context => true;
//    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
//});
//builder.Services.AddAuthentication(
//    CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie();
#endregion


//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<DataService>();
//builder.Services.AddScoped<DataService>();
builder.Services.AddHostedService<DataBackgroundService>();
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllers()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "V1" });
});
builder.Services.AddDevExpressBlazor();

builder.Services.AddHttpClient();

//user
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthentication>();
builder.Services.AddScoped<IAuthDataService, UserDataService>();

// Register CultureService
builder.Services.AddScoped<CultureService>();

var app = builder.Build();

// Configure supported cultures
var supportedCultures = new[] { "zh-TW", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("zh-TW")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");
});

// Use request localization
app.UseRequestLocalization(localizationOptions);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"\\172.25.210.8\01.product$\411 Standard Operation Procedure"),
    RequestPath = "/esop"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"\\172.25.210.8\01.product$"),
    RequestPath = "/mainesop"
});


//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

app.UseCookiePolicy();
app.UseAuthentication();

app.Run();