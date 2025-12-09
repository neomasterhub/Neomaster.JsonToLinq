using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Neomaster.JsonToLinq.Demo;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton<Menu>();

var app = builder.Build();
app.AddCommand((Menu menu) => menu.Show());
app.Run();
