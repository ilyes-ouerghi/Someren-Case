using Someren.Repositories;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Ensure appsettings.json is loaded
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Pass IConfiguration to services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register RoomRepository
builder.Services.AddScoped<IRoomRepository, DbRoomRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
