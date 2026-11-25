using UmbracoExam17.Domain;
using UmbracoExam17.Models;
using UmbracoExam17.Domain.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

builder.Services.Configure<GlobalSetting>(builder.Configuration.GetSection("GlobalSetting"));
builder.Services.AddScoped<IReadingTimeService, ReadingTimeService>();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
