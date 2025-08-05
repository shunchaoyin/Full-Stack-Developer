using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FrontEndApp;
using FrontEndApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to point to the ServerApi
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5275") });

// Register the WeatherService and WeatherState
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<WeatherState>();

// Register the ChatService
builder.Services.AddSingleton<ChatService>();

await builder.Build().RunAsync();
