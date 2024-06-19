using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using RecipeFinder_WebApp;
using RecipeFinder_WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<DataService>()
    .AddSingleton<User>();
builder.Services.AddSingleton<ScrapperService>();
builder.Services.AddScoped<UserService>();

//builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersProfileDBConnection")));

// Register ApplicationDbContext with connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ApplicationDbContext as a scoped service
builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = builder.Configuration.GetValue<bool>("DetailedErrors");
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();