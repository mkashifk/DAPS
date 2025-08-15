using IdentityService.Api.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseWrapperFilter>();
});

// Database
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger/OpenAPI
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();  // required for Swagger
builder.Services.AddSwaggerGen();              // add Swagger generator

var app = builder.Build();

app.UseSwagger();                             // enable Swagger middleware
app.UseSwaggerUI();                           // enable Swagger UI

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Uncomment if you want HTTPS redirection
// app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
