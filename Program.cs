using Microsoft.EntityFrameworkCore;
using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using concesionaria_menichetti.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddControllersWithViews();

//Servicio de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>//el sitio web valida con cookie
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Home/Restringido";
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);//Tiempo de expiración
    });
// .AddJwtBearer(options =>//la api web valida con token
// {
//     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = configuration["TokenAuthentication:Issuer"],
//         ValidAudience = configuration["TokenAuthentication:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
//             configuration["TokenAuthentication:SecretKey"])),
//     };
//     // opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
//     // options.Events = new JwtBearerEvents
//     // {
//     //     OnMessageReceived = context =>
//     //     {
//     //         // Leer el token desde el query string
//     //         var accessToken = context.Request.Query["access_token"];
//     //         // Si el request es para el Hub u otra ruta seleccionada...
//     //         var path = context.HttpContext.Request.Path;
//     //         if (!string.IsNullOrEmpty(accessToken) &&
//     //             (path.StartsWithSegments("/chatsegurohub") ||
//     //             path.StartsWithSegments("/api/propietarios/reset") ||
//     //             path.StartsWithSegments("/api/propietarios/token")))
//     //         {//reemplazar las urls por las necesarias ruta ⬆
//     //             context.Token = accessToken;
//     //         }
//     //         return Task.CompletedTask;
//     //     },
//     //     OnTokenValidated = context =>
//     //     {
//     //         // Este evento se activa cuando el token es validado correctamente
//     //         Console.WriteLine("Token válido para el usuario: " + context?.Principal?.Identity?.Name);
//     //         // Aquí puedes realizar otras validaciones o acciones si es necesario
//     //         return Task.CompletedTask;
//     //     },
//     //     OnAuthenticationFailed = context =>
//     //     {
//     //         // Este evento se activa cuando la autenticación falla
//     //         Console.WriteLine("Error en la autenticación del token: " + context.Exception.Message);
//     //         return Task.CompletedTask;
//     //     }
//     // };
// });

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});

//logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddScoped<VehiculoRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ConcesionariaRepository>();
builder.Services.AddScoped<PlanesConcesionariaRepository>();
builder.Services.AddScoped<SuscripcionesRepository>();
builder.Services.AddScoped<FotosVehiculoRepository>();
builder.Services.AddScoped<PagoRepository>();
builder.Services.AddScoped<DestacadoRepository>();
builder.Services.AddScoped<ContratoSuscripcionRepository>();
builder.Services.AddScoped<ContratoPlanesRepository>();
builder.Services.AddScoped<HomeRepository>();
builder.Services.AddScoped<FavoritoRepository>();
builder.Services.AddScoped<AccesosPagadoRepository>();



//Agregar el contexto de la base de datos a la inyección de dependencias
builder.Services.AddDbContext<ConcesionariaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)) // Usá tu versión de MySQL
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticación
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
