using TodoList.Data;
using TodoList.Repository;

var builder = WebApplication.CreateBuilder(args);

// Porta dinâmica do Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// Configuração
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados
builder.Services.AddScoped<DbSession>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new DbSession(configuration);
});

// Repositórios
builder.Services.AddTransient<ITarefaRepository, TarefaRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Swagger habilitado em Dev e Produção
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
