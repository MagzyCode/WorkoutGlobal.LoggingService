using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.LoggingService.Api.Consumers;
using WorkoutGlobal.LoggingService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<CreateLogConsumer>();

    options.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqHost"]);

        cfg.ReceiveEndpoint(builder.Configuration["Exchanges:CreateLog"], endpoint =>
        {
            endpoint.ConfigureConsumer<CreateLogConsumer>(cxt);
        });
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
