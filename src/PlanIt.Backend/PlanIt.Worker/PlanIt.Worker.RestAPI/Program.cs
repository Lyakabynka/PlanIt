using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PlanIt.Worker.Application;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.RestAPI.DependencyInjection;
using PlanIt.Worker.RestAPI.Middleware;

// Allow DateTime for postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomConfigurations(builder.Configuration);

builder.Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder.Services.AddApplication();
// builder.Services.AddPersistence();

builder.Services.AddCors(options =>
{
    // All clients (temporary)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
    
    //TODO: Change it
    options.AddPolicy("React", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.WithOrigins("http://localhost:3000");
        policy.AllowCredentials();
    });
});

// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//         options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//
//         //options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<PlanType>());
//     });

//swagger
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
    
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Cookie,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

var app = builder.Build();

app.UseCustomExceptionHandler();
app.UseJwtTokenExtractor();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("React");

// app.UseSwagger();
// app.UseSwaggerUI(config =>
// {
//     // show swagger page using root Uri
//     config.RoutePrefix = string.Empty;
//
//     config.SwaggerEndpoint("swagger/v1/swagger.json", "PlanIt Identity PlanIt.Worker.RestAPI");
// });

app.MapHub<PlanHub>("plan-hub");

// app.MapControllers();

app.Run();