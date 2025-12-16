using Cocona;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neomaster.JsonToLinq.Demo;

var builder = CoconaApp.CreateBuilder();
var config = builder.Configuration;
AppDbContext.DefaultConnectionString = config.GetConnectionString("Default");

builder.Services.AddScoped<Menu>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<UserDemoService>();

var app = builder.Build();

app.AddCommand((Menu menu) => menu.Show());
app.Run();
