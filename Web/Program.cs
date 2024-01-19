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
builder.Services.AddMudBlazorDialog();

//builder.Services.AddScoped(sp =>
//    new HttpClient
//    {
//        //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) //TDO edit to add url here or make config file
//        BaseAddress = new Uri("http://localhost:5000"),
//    });

builder.Services
    .AddTransient<CookieHandler>()
    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"))
    .AddHttpClient("API", client => client.BaseAddress = new Uri("http://localhost:5000"))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<NovelService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CharacterNameService>();
builder.Services.AddScoped<NovelUserService>();

await builder.Build().RunAsync();