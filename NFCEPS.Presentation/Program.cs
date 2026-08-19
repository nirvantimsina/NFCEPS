using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NFCEPS.Application.Helpers;
using NFCEPS.Presentation.Middleware;
using NFCEPS.Infrastructure.Repositories;
using NFCEPS.Application.Interfaces;
using Scalar.AspNetCore;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);


//JWT Settings
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JWTSettings>()!;
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<JWTHelper>();

// add the services here below
builder.Services.AddSingleton<PermissionService>();
builder.Services.AddScoped<IGenericRepository, GenericRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IGenericRepository).Assembly));

//connection string
var connectionString = builder.Configuration
    .GetConnectionString("NFCEPS_DB")!;
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

//controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// swagger with JWT support
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token"
        });
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
        return Task.CompletedTask;
    });
});


//JWT Auth
var secretKey = jwtSettings.SecretKey ?? throw new InvalidOperationException("JWT SecretKey is missing in configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });


builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7183", "http://localhost:5088")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// load permissions at startup
using (var scope = app.Services.CreateScope())
{
    var permService = scope.ServiceProvider
        .GetRequiredService<PermissionService>();
    await permService.LoadAsync();
}

// Hydrate cache on application startup
using (var startupScope = app.Services.CreateScope())
{
    var permService = startupScope.ServiceProvider.GetRequiredService<PermissionService>();
    await permService.LoadAsync();
}

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection(); // maybe later uncomment this i think i might have to test with the https too
app.UseRouting();
app.UseCors("BlazorClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.Run();

// this is for creating password
// var hash = BCrypt.Net.BCrypt.HashPassword("admin123");
// Console.WriteLine(hash);







