using Contacts.Application;
using Contacts.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace Contacts.API.Extensions
{
    public static class ContainerExtensions
    {
        public static void AddControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
        }

        //public static void AddOpenApi(this WebApplicationBuilder builder)
        //{
        //    builder.Services.AddOpenApi();
        //}

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer` Generated-JWT-Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            new string[]{}
                        }
                    }
                );


                #region XML documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                #endregion

            });
        }

        public static void AddApplicationLayer(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplication();
        }

        public static void AddInfrastructureLayer(this WebApplicationBuilder builder)
        {
            builder.Services.AddInfrastructure(builder.Configuration);
        }

        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            // Read from appsettings + environment-specific settings
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });
        }

        public static void AddAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                  options.Authority = builder.Configuration["Jwt:Authority"];
                  options.RequireHttpsMetadata = false;

                  var validAudiences = builder.Configuration.GetSection("Jwt:Audience").Get<string[]>();
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidAudiences = validAudiences
                  };
              });
        }

        public static void AddAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization();
        }
    }
}
