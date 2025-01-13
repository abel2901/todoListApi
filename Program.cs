using Microsoft.Extensions.DependencyInjection;
using TodoList.Data;
using TodoList.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<ITarefaRepository, TarefaRepository>();


var app = builder.Build();

//var port = Environment.GetEnvironmentVariable("PORT") ?? "50900";
//app.Urls.Add($"http://*:{port}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
