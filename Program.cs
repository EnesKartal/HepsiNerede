using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<HepsiNeredeDBContext>(options =>
{
    //get configuration from appsettings.json
    var hepsiNeredeConnectionString = configuration.GetConnectionString("HepsiNeredeDBContext");
    options.UseSqlite(hepsiNeredeConnectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HepsiNeredeDBContext>();
    dbContext.Database.EnsureCreated();
}

app.MapGet("/", () => "Hello World!");

app.Run();
