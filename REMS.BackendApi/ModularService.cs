using Microsoft.EntityFrameworkCore;
using REMS.Database.AppDbContextModels;
using REMS.Modules.Features.Agent;

namespace REMS.BackendApi
{
    public static class ModularService
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            return builder;
        }

        public static WebApplicationBuilder AddDbService(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);
            return builder;
        }

        public static WebApplicationBuilder AddDataAccessService(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DA_Agent>();
            return builder;
        }

        public static WebApplicationBuilder AddBusinessLogicService(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<BL_Agent>();
            return builder;
        }
    }
}
