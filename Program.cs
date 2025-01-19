using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

using MySql.Data.EntityFrameworkCore.Extensions;




var builder = WebApplication.CreateBuilder(args);

// Configurar el DbContext para usar MySQL
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDbContext<WorldDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("WorldDbContext"),
    new MySqlServerVersion(new Version(8, 0, 25))));




// Agregar servicios de controladores
builder.Services.AddControllers();




// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
