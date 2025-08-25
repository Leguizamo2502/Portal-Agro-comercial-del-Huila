using CloudinaryDotNet;
using Entity.Domain.Models.Implements.Auth.Token;
using Microsoft.Extensions.FileProviders;
using Web.ProgramService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCustomCors(builder.Configuration);

//Jwt
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("Cookie"));

//Cloudinary
var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");

var cloudinary = new Cloudinary(new Account(
    cloudinaryConfig["CloudName"],
    cloudinaryConfig["ApiKey"],
    cloudinaryConfig["ApiSecret"]

));

builder.Services.AddSingleton(cloudinary);


//Services
builder.Services.AddApplicationServices();

//Database
builder.Services.AddDatabase(builder.Configuration);




var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Archivos estáticos
app.UseStaticFiles();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PortalAgro API v1");
    c.RoutePrefix = "swagger";
});



app.UseHttpsRedirection();

// ?? 1. Primero CORS (para que los headers y cookies se acepten)
app.UseCors();

// ?? 2. Luego autenticación (JWT desde cookie)
app.UseAuthentication();

// ?? 3. Después autorización
app.UseAuthorization();

// ?? 4. Finalmente, los controladores
app.MapControllers();

app.Run();
