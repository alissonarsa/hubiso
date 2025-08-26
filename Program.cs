using hubiso.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization; // <-- ADICIONAR 1: Para trabalhar com culturas
using Microsoft.AspNetCore.Localization; // <-- ADICIONAR 2: Para trabalhar com localização

var builder = WebApplication.CreateBuilder(args);

// Adicionar a Connection String e o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- INÍCIO DA NOVA CONFIGURAÇÃO DE CULTURA ---
var supportedCultures = new[] { new CultureInfo("pt-BR") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);
// --- FIM DA NOVA CONFIGURAÇÃO DE CULTURA ---


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // O MapStaticAssets() que você tinha foi atualizado para isto em versões mais recentes

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();