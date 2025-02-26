using Presentation.Configurations.Apps;
using Presentation.Configurations.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

app.AddAllApps();
app.Run();
