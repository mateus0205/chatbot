var builder = WebApplication.CreateBuilder(args);
// Serviços
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        p => p.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Middlewares
app.UseCors("AllowAll");

app.MapControllers();

app.Run();