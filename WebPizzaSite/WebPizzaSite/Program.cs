using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Constants;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Data.Entities.Identity;
using WebPizzaSite.Interfaces;
using WebPizzaSite.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IImageWorker, ImageWorker>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PizzaDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity options
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;

    //options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<PizzaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<PizzaDbContext>();
    var userManager = serviceScope.ServiceProvider.GetService<UserManager<UserEntity>>();
    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<RoleEntity>>();
    var imageWorker = serviceScope.ServiceProvider.GetService<IImageWorker>();

    context?.Database.Migrate();

    //���� ������� ������

    if (!context.Categories.Any())
    {
        string url = "https://loremflickr.com/1200/800/tokio,cat/all";
        var faker = new Faker("uk");
        var categories = faker.Commerce.Categories(10);
        foreach (var categoryName in categories)
        {
            string fileName = imageWorker.ImageSave(url);
            if (!string.IsNullOrEmpty(fileName))
            {

                var entity = new CategoryEntity
                {
                    Name = categoryName,
                    Description = faker.Lorem.Lines(5),
                    Image = fileName
                };
                context.Categories.Add(entity);
                context.SaveChanges();
            }
        }
    }

    if (!context.Products.Any())
    {
        string url = "https://loremflickr.com/1200/800/car/all";
        var faker = new Faker("uk");

        var catCount = context.Categories.Count();
        if (catCount != 0)
        {

            var catIds = context.Categories.Select(x => x.Id).ToList();
            int propductCount = 100;
            for (int k = 0; k < propductCount; k++)
            {
                var catIndex = faker.Random.Number(0, catCount - 1);
                var p = new ProductEntity
                {
                    CategoryId = catIds[catIndex],
                    Name = faker.Commerce.ProductName(),
                    Price = decimal.Parse(faker.Commerce.Price())
                };
                context.Add(p);
                int countImages = faker.Random.Number(3, 5);
                for (int i = 0; i < countImages; i++)
                {
                    string fileName = imageWorker.ImageSave(url);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var pi = new ProductImageEntity
                        {
                            Name = fileName,
                            Priority = i,
                            Product = p
                        };
                        context.Add(pi);
                    }
                }
                context.SaveChanges();
            }
        }
    }

    if (!context.Roles.Any())
    {
        var admin = new RoleEntity
        {
            Name = Roles.Admin
        };
        var result = roleManager.CreateAsync(admin).Result;
        if (!result.Succeeded)
        {
            Console.WriteLine($"------������� ��������� ��� {Roles.Admin}------");
        }

        result = roleManager.CreateAsync(new RoleEntity { Name = Roles.User }).Result;
        if (!result.Succeeded)
        {
            Console.WriteLine($"------������� ��������� ��� {Roles.User}------");
        }
    }

    if (!context.Users.Any())
    {
        var user = new UserEntity
        {
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            LastName = "�����",
            FirstName = "������",
            Picture = "amdin.jpg"
        };
        var result = userManager.CreateAsync(user, "123456").Result;
        if (result.Succeeded)
        {
            result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
            if (!result.Succeeded)
            {
                Console.WriteLine($"-------�� ������� ������ ���� {Roles.Admin} ����������� {user.Email}------");
            }
        }
        else
        {
            Console.WriteLine($"-------�� ������� �������� ����������� {user.Email}-------");
        }
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Main}/{action=Index}/{id?}");
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "admin_area",
        areaName: "Admin",
        pattern: "/admin/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Main}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.Run();
