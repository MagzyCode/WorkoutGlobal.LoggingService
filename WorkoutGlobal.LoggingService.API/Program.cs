using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.LoggingService.Api.Enums;
using WorkoutGlobal.LoggingService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var broker = Enum.Parse<Broker>(builder.Configuration["MassTransitSettings:Bus"]);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.ConfigureValidators();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureRepositories();

builder.Services.ConfigureMassTransit(builder.Configuration, broker);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
