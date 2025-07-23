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


//Services
builder.Services.AddApplicationServices();

//Database
builder.Services.AddDatabase(builder.Configuration);




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

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
