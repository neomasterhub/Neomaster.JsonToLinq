using Cocona;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neomaster.JsonToLinq.Demo;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton<Menu>();

var app = builder.Build();
builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql("Host=localhost;Port=5432;Database=JsonToLinqDemo;Username=postgres;Password=postgres");
});
app.AddCommand((Menu menu) => menu.Show());
app.Run();
