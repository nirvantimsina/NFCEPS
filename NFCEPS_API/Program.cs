using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NFCEPS_API.Auth;
using NFCEPS_API.Repository;
using NFCEPS_API.Services.Permission;
using NFCEPS_API.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);


//JWT Settings
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JWTSettings>()!;
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<JWTHelper>();

// add the services here below
builder.Services.AddSingleton<PermissionService>();
builder.Services.AddScoped<IGenericRepository>();

//Connection String
var connectionString = builder.Configuration
    .GetConnectionString("NFCEPS_DB")!;
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

//Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference("Bearer")
        ] = new List<string>()
    });
});

//JWT Auth
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

// load permissions at startup
using (var scope = app.Services.CreateScope())
{
    var permService = scope.ServiceProvider
        .GetRequiredService<PermissionService>();
    await permService.LoadAsync();
}

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // This creates the /swagger UI page
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("BlazorClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.Run();