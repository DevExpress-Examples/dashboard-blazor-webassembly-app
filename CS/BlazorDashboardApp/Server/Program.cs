using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

IFileProvider fileProvider = builder.Environment.ContentRootFileProvider;
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDevExpressControls();
builder.Services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
    DashboardConfigurator configurator = new DashboardConfigurator();
    // Register Dashboard Storage
    configurator.SetDashboardStorage(new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboards").PhysicalPath));
    // Create a sample JSON data source
    DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
    DashboardJsonDataSource jsonDataSourceUrl = new DashboardJsonDataSource("JSON Data Source (URL)");
    jsonDataSourceUrl.JsonSource = new UriJsonSource(
            new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
    jsonDataSourceUrl.RootElement = "Customers";
    dataSourceStorage.RegisterDataSource("jsonDataSourceUrl", jsonDataSourceUrl.SaveToXml());
    configurator.SetDataSourceStorage(dataSourceStorage);
    return configurator;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseDevExpressControls();
app.UseRouting();
EndpointRouteBuilderExtension.MapDashboardRoute(app, "api/dashboard", "DefaultDashboard");

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
