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
//builder.Services.AddJwtAuthentication(builder.Configuration);





//Services
builder.Services.AddApplicationServices();

//Database
builder.Services.AddDatabase(builder.Configuration);


builder.Services.AddAuthorization();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();// Usar autentificación de JWT

app.UseCors();

app.UseMiddleware<JwtCookieMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
