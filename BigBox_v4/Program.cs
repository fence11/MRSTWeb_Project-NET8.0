using BigBox_v4.BusinessLogic;
using BigBox_v4.Data;
using BigBox_v4.Domain;
using BigBox_v4.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

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

builder.Services.AddScoped<IWebSessionRepository, WebSessionRepository>();

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

app.UseWebSessions();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
     var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

     // Apply any pending migrations
     db.Database.Migrate();

     // Only add admin if not already present
     if (!db.Users.Any(u => u.IsAdmin))
     {
          var adminUser = new User
          {
               Username = "admin",
               FirstName = "Admin",
               LastName = "User",
               Email = "admin@bigbox.com",
               IsAdmin = true
          };

          // Manually create a hasher and hash the password
          var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
          adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");

          db.Users.Add(adminUser);
          db.SaveChanges();
     }
}


app.Run();

