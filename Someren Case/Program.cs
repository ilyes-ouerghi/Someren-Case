using Someren_Case.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Add services to the container
builder.Services.AddControllersWithViews();

// Register the student repository with dependency injection
string connectionString = configuration.GetConnectionString("dbproject242504");

// Register DbStudentRepository for IStudentRepository as Scoped (one per HTTP request)
builder.Services.AddScoped<IStudentRepository>(provider => new DbStudentRepository(connectionString));

// Register DbActivityParticipantRepository for IActivityParticipantRepository as Scoped (one per HTTP request)
builder.Services.AddScoped<IActivityParticipantRepository>(provider => new DbActivityParticipantRepository(connectionString));

// Register DbActivityRepository for IActivityRepository as Scoped (one per HTTP request)
builder.Services.AddScoped<IActivityRepository>(provider => new DbActivityRepository(connectionString));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();