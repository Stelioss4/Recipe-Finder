using RecipeFinder_WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<DataService>()
    .AddSingleton<User>();

// Read the API key from configuration
var apiKey = builder.Configuration["Spoonacular:a1f6b23d83fb40ec877e2e2b9adcfe49"];

// Register HttpClient with base address for Spoonacular API
builder.Services.AddHttpClient("SpoonacularClient", client =>
{
    client.BaseAddress = new Uri("https://api.spoonacular.com/");
    client.DefaultRequestHeaders.Add("a1f6b23d83fb40ec877e2e2b9adcfe49", apiKey); // Adding API key as a default header
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