using BigBox_v4.BusinessLogic;
using BigBox_v4.Data;
using BigBox_v4.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IBusinessLogic<>), typeof(BusinessService<>));

builder.Services.AddScoped<IDriversRepository, DriversRepository>();
builder.Services.AddScoped<IDriversBusinessLogic, DriversBusinessLogic>();

builder.Services.AddScoped<IDriverScheduleRepository, DriverScheduleRepository>();
builder.Services.AddScoped<IDriverScheduleBusinessLogic, DriverScheduleBusinessLogic>();

builder.Services.AddScoped<IRepository<Truck>, Repository<Truck>>();

// Register box repositories and business logic
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IBoxBusinessLogic, BoxBusinessLogic>();
builder.Services.AddScoped<IBoxSizeRepository, BoxSizeRepository>();
builder.Services.AddScoped<IBoxSizeBusinessLogic, BoxSizeBusinessLogic>();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

