using Entity.Domain.Models.Implements.Auth.Token;
using Entity.Validation.Service;
using Entity.Validations.interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Web.ProgramService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCustomCors(builder.Configuration);

//Validations
builder.Services.AddScoped<IValidatorService, ValidatorService>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

//Jwt y cookies
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("Cookie"));

//Cloudinary
builder.Services.AddCloudinaryServices(builder.Configuration);

//Services
builder.Services.AddApplicationServices();

//Database
builder.Services.AddDatabase(builder.Configuration);

//Background Services
builder.Services.AddBackgroundServices(builder.Configuration);  

//Cache
builder.Services.AddOutputCachePolicies();

var app = builder.Build();

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

// ?? 1. Primero CORS 
app.UseCors();

// ?? 2. Luego autenticación 
app.UseAuthentication();

// ?? 3. Después autorización
app.UseAuthorization();

//cache
app.UseOutputCache();
// ?? 4. Finalmente, los controladores
app.MapControllers();

// ?? 5. MIGRACIONES EN ARRANQUE
MigrationManager.MigrateAllDatabases(app.Services, builder.Configuration);

app.Run();
