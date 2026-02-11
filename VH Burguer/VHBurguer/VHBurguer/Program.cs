var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Explicacoes das pastas:
// DTOs(Data Transfer Objects): Vai conter classes que serao usadas em diversas camadas
// Aplications: Contera a logica principal do projeto
    //Service: Contem as regras de négócios
// Repositories: Contem as implementacoes da interface
// Context: Contem a logica do banco de dados

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
