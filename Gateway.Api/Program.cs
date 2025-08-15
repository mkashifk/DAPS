using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();  // required for Swagger
builder.Services.AddSwaggerGen();              // add Swagger generator

var app = builder.Build();

app.UseSwagger();                             // enable Swagger middleware
app.UseSwaggerUI();                           // enable Swagger UI

app.UseRouting();

// Add Ocelot middleware
await app.UseOcelot();

app.Run();
