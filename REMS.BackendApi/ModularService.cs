using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REMS.Shared;

namespace REMS.BackendApi;

public static class ModularService
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder;
    }

    public static WebApplicationBuilder AddDbService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(
            opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")); },
            ServiceLifetime.Transient, ServiceLifetime.Transient);
        return builder;
    }

    public static WebApplicationBuilder AddDataAccessService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DA_Agent>();
        builder.Services.AddScoped<DA_Appointment>();
        builder.Services.AddScoped<DA_Client>();
        builder.Services.AddScoped<DA_Property>();
        builder.Services.AddScoped<DA_Review>();
        builder.Services.AddScoped<DA_Transaction>();
        builder.Services.AddScoped<DA_Signin>();
        builder.Services.AddScoped<JwtTokenService>();
        return builder;
    }

    public static WebApplicationBuilder AddBusinessLogicService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<BL_Agent>();
        builder.Services.AddScoped<BL_Appointment>();
        builder.Services.AddScoped<BL_Client>();
        builder.Services.AddScoped<BL_Property>();
        builder.Services.AddScoped<BL_Review>();
        builder.Services.AddScoped<BL_Transaction>();
        builder.Services.AddScoped<BL_Signin>();
        return builder;
    }

    public static WebApplicationBuilder AddJwtAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "REMS", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });
        var issuer = builder.Configuration["Jwt:Issuer"];
        var validAudience = builder.Configuration["Jwt:Audience"];
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddAuthorization();
        return builder;
    }
}