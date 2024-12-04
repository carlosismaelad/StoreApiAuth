using StoreApiAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddControllers();

builder.Services.AddSingleton<PostgresStatusService>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new PostgresStatusService(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
// Adicionando a rota "/" para retornar "API ok!"
app.MapGet("/", () => Results.Ok("API ok!"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Executando o aplicativo
app.Run();
