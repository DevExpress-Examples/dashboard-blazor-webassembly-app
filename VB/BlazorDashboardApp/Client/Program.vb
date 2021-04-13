Imports Microsoft.AspNetCore.Components.WebAssembly.Hosting
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports System
Imports System.Collections.Generic
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks

Namespace BlazorDashboardApp.Client
	Public Class Program
		Public Shared Async Function Main(ByVal args() As String) As Task
			Dim builder = WebAssemblyHostBuilder.CreateDefault(args)
			builder.RootComponents.Add(Of App)("#app")

			builder.Services.AddScoped(Function(sp) New HttpClient With {.BaseAddress = New Uri(builder.HostEnvironment.BaseAddress)})

			Await builder.Build().RunAsync()
		End Function
	End Class
End Namespace
