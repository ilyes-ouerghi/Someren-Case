using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Someren_Case.Interfaces;
using Someren_Case.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("dbproject242504");

// Register MVC services
builder.Services.AddControllersWithViews();

// Register repositories (with ADO.NET, no frameworks)
builder.Services.AddScoped<ILecturerRepository>(sp => new DbLecturerRepository(connectionString));
builder.Services.AddScoped<IStudentRepository>(provider => new DbStudentRepository(connectionString));
builder.Services.AddScoped<IDrinkRepository>(provider => new DbDrinkRepository(connectionString));

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default route remains Lecturer/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lecturer}/{action=Index}/{id?}"
);

app.Run();
