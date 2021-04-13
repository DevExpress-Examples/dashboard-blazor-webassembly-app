﻿Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.HttpsPolicy
Imports Microsoft.AspNetCore.ResponseCompression
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports System.Linq
Imports DevExpress.AspNetCore
Imports DevExpress.DashboardAspNetCore
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWeb
Imports DevExpress.DataAccess.Json
Imports Microsoft.Extensions.FileProviders
Imports System

Namespace BlazorDashboardApp.Server
	Public Class Startup
		Public Sub New(ByVal configuration As IConfiguration, ByVal hostingEnvironment As IWebHostEnvironment)
			Me.Configuration = configuration
			FileProvider = hostingEnvironment.ContentRootFileProvider
		End Sub

		Public ReadOnly Property Configuration() As IConfiguration
		Public ReadOnly Property FileProvider() As IFileProvider

		' This method gets called by the runtime. Use this method to add services to the container.
		' For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		Public Sub ConfigureServices(ByVal services As IServiceCollection)

			services.AddControllersWithViews()
			services.AddRazorPages()
			services.AddDevExpressControls()
			services.AddMvc().AddDefaultDashboardController(Sub(configurator)
				configurator.SetDashboardStorage(New DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath))
				Dim dataSourceStorage As New DataSourceInMemoryStorage()
				Dim jsonDataSourceUrl As New DashboardJsonDataSource("JSON Data Source (URL)")
				jsonDataSourceUrl.JsonSource = New UriJsonSource(New Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"))
				jsonDataSourceUrl.RootElement = "Customers"
				dataSourceStorage.RegisterDataSource("jsonDataSourceUrl", jsonDataSourceUrl.SaveToXml())
				configurator.SetDataSourceStorage(dataSourceStorage)
			End Sub)
		End Sub

		' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IWebHostEnvironment)
			If env.IsDevelopment() Then
				app.UseDeveloperExceptionPage()
				app.UseWebAssemblyDebugging()
			Else
				app.UseExceptionHandler("/Error")
				' The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts()
			End If

			app.UseHttpsRedirection()
			app.UseBlazorFrameworkFiles()
			app.UseStaticFiles()
			app.UseDevExpressControls()
			app.UseRouting()

			app.UseEndpoints(Sub(endpoints)
				EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard")
				endpoints.MapRazorPages()
				endpoints.MapControllers()
				endpoints.MapFallbackToFile("index.html")
			End Sub)
		End Sub
	End Class
End Namespace
