using Someren_Case.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register DbLecturerRepository for ILecturerRepository with dependency injection
builder.Services.AddScoped<ILecturerRepository>(provider =>
    new DbLecturerRepository(provider.GetRequiredService<IConfiguration>())); // Inject IConfiguration

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lecturers}/{action=Index}/{id?}"
);

app.Run();
