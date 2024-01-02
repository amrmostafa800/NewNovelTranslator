using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Web;
using Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) //TDO edit to add url here or make config file
        BaseAddress = new Uri("https://localhost:7064")
    });

builder.Services.AddScoped<NovelService>();

await builder.Build().RunAsync();
