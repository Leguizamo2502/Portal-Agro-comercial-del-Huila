using Business;
using Data;
using Entity.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//person
builder.Services.AddScoped<PersonData>();  // Asegúrate de que PersonaData está registrado
builder.Services.AddScoped<PersonBusiness>(); // Registra la capa de negocio
//Form
builder.Services.AddScoped<FormData>();
builder.Services.AddScoped<FormBusiness>();
//Permission
builder.Services.AddScoped<PermissionData>();
builder.Services.AddScoped<PermissionBusiness>();
//Rol
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<RolBusiness>();
//Module
builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleData>();
//User
builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserData>();
//RolUser
builder.Services.AddScoped<RolUserBusiness>();
builder.Services.AddScoped<RolUserData>();
//FormModule
builder.Services.AddScoped<FormModuleBusiness>();
builder.Services.AddScoped<FormModuleData>();
//RolFormPermission
builder.Services.AddScoped<RolFormPermissionBusiness>();
builder.Services.AddScoped<RolFormPermissionData>();
//FormModule
builder.Services.AddScoped<FormModuleBusiness>();
builder.Services.AddScoped<FormModuleData>();


// Agrega el sistema de logs


var OrigenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(politica =>
    {
        politica.WithOrigins(OrigenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
});

//builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones
//.UseSqlServer("name=DefaultConnection"));
var configuration = builder.Configuration;
var provider = configuration.GetValue<string>("DatabaseProvider");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (provider == "SqlServer")
    {
        options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
    }
    else if (provider == "PostgreSql")
    {
        options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
    }
    else if (provider == "MySql")
    {
        var connectionString = configuration.GetConnectionString("MySqlConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        options.UseMySql(connectionString, serverVersion);
    }
    else
    {
        throw new Exception("Base de datos no encontrada");
    }
});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
