using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); 
// Configura la autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login"; // Ruta de login
                    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Tiempo de expiraci�n de la cookie
                    options.SlidingExpiration = true; // Si se renueva la expiraci�n en cada solicitud
                    options.Cookie.HttpOnly = true; // La cookie no est� accesible v�a JavaScript
                    options.Cookie.SameSite = SameSiteMode.Lax;
                });
builder.Environment.EnvironmentName = "Development";
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Agrega la autenticaci�n al pipeline
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");

app.Run();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Muestra detalles completos de la excepci�n en el navegador
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}