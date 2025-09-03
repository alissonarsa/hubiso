using hubiso.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configura um HttpClient nomeado especificamente para a BrasilAPI
builder.Services.AddHttpClient("BrasilApi", client =>
{
    client.BaseAddress = new Uri("https://brasilapi.com.br/");
    client.DefaultRequestHeaders.Add("User-Agent", "hubiso-app");
});

// Adicionar a Connection String e o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Garante que todas as respostas JSON da API usem camelCase
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// --- Configuração de Cultura para pt-BR ---
var supportedCultures = new[] { new CultureInfo("pt-BR") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);
// --- Fim da Configuração de Cultura ---

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Adicionado pelo Copilot - Garante que as rotas por atributo sejam mapeadas
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();