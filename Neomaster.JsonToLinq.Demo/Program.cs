using Cocona;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neomaster.JsonToLinq.Demo;

var builder = CoconaApp.CreateBuilder();
var config = builder.Configuration;
var cs = config.GetConnectionString("Demo");

builder.Services.AddScoped<Menu>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(cs));

var app = builder.Build();

app.AddCommand((Menu menu) => menu.Show());
app.Run();
