using Infrastructure.Seed;
using Microsoft.Extensions.Options;
using TbcTaskApi.Configurations;
using TbcTaskApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplication(builder.Configuration, builder.Environment, builder);
builder.Services.ConfigureAutoMapper();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateRequestFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ApplicationConfigurations.UserLocalization(app);

DbInitializer.Initialize(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.UseCustomExceptionLoggerMiddleware(builder.Environment, logger);

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();
    
app.Run();