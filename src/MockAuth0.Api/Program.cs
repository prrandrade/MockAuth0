using MockAuth0.Api.Models;
using MockAuth0.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("configs/config.json", true, true);

// configurations
builder.Services.AddSingleton(builder.Configuration.GetSection("Organizations").Get<List<OrganizationConfigurationModel>>());
builder.Services.AddSingleton(builder.Configuration.GetSection("Addresses").Get<AddressesModel>());


builder.Services.AddSingleton<IJwtGeneratorService, JwtGeneratorService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();