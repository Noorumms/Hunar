using Hunar.Application.UseCases.Auth;
using Hunar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hunar.Domain.Interfaces;
using Hunar.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hunar.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ?? DATABASE ?????????????????????????????????????????????????????
builder.Services.AddDbContext<HunarDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// ?? REPOSITORIES ?????????????????????????????????????????????????
// "When anyone asks for IUserRepository, give them UserRepository"
// Scoped = one instance per HTTP request — correct for DB operations
builder.Services.AddScoped<IUserRepository, UserRepository>();
// ?? SERVICES ?????????????????????????????????????????????????????
builder.Services.AddScoped<IJwtService, JwtService>();

// ?? USE CASES ????????????????????????????????????????????????????
// Register each use case so controllers can receive them
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<LoginUseCase>();

// ?? JWT AUTHENTICATION ????????????????????????????????????????????
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    // Set JWT as the default authentication scheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,           // token must come from our server
        ValidateAudience = true,         // token must be meant for our app
        ValidateLifetime = true,         // reject expired tokens
        ValidateIssuerSigningKey = true, // signature must match our secret
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // This adds the Authorize button to Swagger UI
    // So we can test protected endpoints directly in browser
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token here"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ?? MIDDLEWARE PIPELINE ???????????????????????????????????????????
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ORDER MATTERS — Authentication before Authorization always
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();