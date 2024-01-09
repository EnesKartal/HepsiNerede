using HepsiNerede.Data;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Services;
using HepsiNerede.WebApp.Core.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<HepsiNeredeDBContext>(options =>
{
    var hepsiNeredeConnectionString = configuration.GetConnectionString("HepsiNeredeDBContext");
    options.UseSqlite(hepsiNeredeConnectionString);
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton<ITimeSimulationService, TimeSimulationService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HepsiNerede API", Version = "v1", });
});


builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HepsiNerede API V1");
    c.RoutePrefix = string.Empty;
});

app.UseErrorHandlingMiddleware();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HepsiNeredeDBContext>();
    dbContext.Database.EnsureCreated();
}


app.MapControllers();

app.Run();
