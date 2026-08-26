using GWS_BlazorServer.Components;
using GWS_BlazorServer.Components.Services;
using GWS_Library;
using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddSingleton<MyAppConfig>();
builder.Services.AddSingleton<HttpClient>();

var app = builder.Build();

// Version 27.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH9fdnRVQ2JdWEx0X0E=");
// Version 31.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfcXVVQ2ZcUkV3WENWYEg=");
// Version
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1fcnRcQ2dcU0Z/XkY=");
// Version 32.x.x
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5edHRURGNeUUx0X0NWYEs=");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.Run();

if (app.Environment.IsDevelopment())
{
    app.Run();
}

if (app.Environment.IsProduction())
{
    app.Run("http://*:5800");
}
