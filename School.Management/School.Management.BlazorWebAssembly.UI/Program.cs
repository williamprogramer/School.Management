using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using School.Management.BlazorWebAssembly.UI;
using School.Management.BlazorWebAssembly.UI.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7265/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<AuthenticationStateProvider, AppAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddMudServices();

await builder.Build().RunAsync();