using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
//builder.Services.AddTransient<IFinnhubService, FinnhubService>();
//builder.Services.AddTransient<IStocksService, StocksService>();

builder.Services.AddTransient<IStocksService, StocksService>();
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IStocksRepository, StocksRepository>();
//builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();   

//app.MapGet("/", () => "Hello World!");

app.Run();
