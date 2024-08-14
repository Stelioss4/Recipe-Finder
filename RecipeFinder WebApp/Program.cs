using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using RecipeFinder_WebApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using RecipeFinder_WebApp.Components.Account;
using RecipeFinder_WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<RecipeFinder_WebAppContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentityCore<RecipeFinder_WebAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RecipeFinder_WebAppContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<RecipeFinder_WebAppUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ScrapperService>();
builder.Services.AddScoped<FavoriteRecipeService>();

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

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

app.MapAdditionalIdentityEndpoints(); 

app.Run();
