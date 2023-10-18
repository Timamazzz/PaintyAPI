using System.Text;
using DataAccess;
using DataAccess.Repositories;
using Domain.AutoMapper;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaintyAPI.AutoMapper;
using PaintyAPI.Services;
using PaintyAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);

//JWT
var configuration = builder.Configuration;

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

//DataBase
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

//Mapping
builder.Services.AddAutoMapper(typeof(AppMappingDomainModelsToDataAccessModelsProfile), 
    typeof(AppMappingPresentationModelsToDomainModelsProfile));

//Services 
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();

//Repositories
builder.Services.AddScoped<UserRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeWell API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();