using HepsiNerede.Data;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<HepsiNeredeDBContext>(options =>
{
    //get configuration from appsettings.json
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

builder.Services.AddSwaggerGen();


builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HepsiNeredeDBContext>();
    dbContext.Database.EnsureCreated();
}


app.MapControllers();

app.Run();
