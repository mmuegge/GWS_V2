using GWS_Statistics;
using GWS_Statistics.Components;
using GWS_Statistics.Components.Services;
using GWS_Statistics.Data;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using System.Globalization;


CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    //.AddInteractiveServerComponents();
    .AddInteractiveServerComponents().AddCircuitOptions(c => c.DetailedErrors = true);
builder.Services.AddSingleton<ToastService>();
builder.Services.AddSingleton<SfDialogService>();
builder.Services.AddSyncfusionBlazor();
//builder.Services.AddServerSideBlazor().AddCircuitOptions(e => { e.DetailedErrors = true; });

builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 108544;
});

//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
// Register the locale service to localize the  SyncfusionBlazor components.
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));

builder.Services.AddSingleton<MyAppConfigModel>();
builder.Services.AddSingleton<HttpClient>();

var app = builder.Build();
app.UseRequestLocalization("de-DE");

// Version 27.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH9fdnRVQ2JdWEx0X0E=");
// Version 28.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1fcnRcQ2dcU0Z/XkY=");
// Version 29.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXxcd3RXR2VdWUxyW0tWYUA=");
// Version 30.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXhfdXRXRmBZV012XEFWYEk=");
// Version 30.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXdfdHRURWleU0J1WUNWYEk=");
// Version 31.x.x
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfcXVVQ2ZcUkV3WENWYEg=");
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

if (app.Environment.IsDevelopment())
{
    app.Run();
}

if (app.Environment.IsProduction())
{
    app.Run("http://*:6050");
}
