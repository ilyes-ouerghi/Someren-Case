using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Someren_Case.Interfaces;
using Someren_Case.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("dbproject242504");

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILecturerRepository>(sp => new DbLecturerRepository(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lecturer}/{action=Index}/{id?}"
);

app.Run();
