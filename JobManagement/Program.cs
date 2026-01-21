using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using JobManagement.Data;
using JobManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JobService>();

//Usado para salvar dados da sessão (última ordenação)
builder.Services.AddSession();

// Assuming the connection string is in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("JobManagementDbContext");

builder.Services.AddDbContext<JobManagementDbContext>(options =>
    options.UseMySql(
        connectionString,
        // Specify the ServerVersion.AutoDetect(connectionString) is a common pattern
        new MySqlServerVersion(ServerVersion.AutoDetect(connectionString)),
        // Configure MySQL-specific options including the migrations assembly
        mysqlOptions => mysqlOptions.MigrationsAssembly("JobManagement")
    )
);

var app = builder.Build();

var ptBR = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
};

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();

    //Chamar o serviço SeedingService
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<JobManagementDbContext>();
            SeedingService seedingService = new SeedingService(context);
            seedingService.Seed();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}

app.UseHttpsRedirection();

app.UseRouting();

//Usado para salvar dados da sessão (última ordenação)
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
