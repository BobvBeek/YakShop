using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the YakDbContext with SQL Server & Swagger
builder.Services.AddDbContext<YakDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YakDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Yakservices to DI container
builder.Services.AddScoped<YakSimulator>();

//CORS policy to allow requests from specific origins
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
