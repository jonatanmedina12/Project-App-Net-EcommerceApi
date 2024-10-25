using API.ConfigurationsFile;
using API.Errors;
using Data;
using Data.ConfigurationJwt;
using Data.Repositories;
using Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Interface;
using System.Text;
using System.Threading.RateLimiting;

namespace API.Extensions
{
    public static class ServiciosAplicacion
    {
        public static IServiceCollection AgregarServiciosAplicacion(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            Services.AddScoped<ConfigJwt>();

            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-commerce API", Version = "v1" });

                // Definición de seguridad para JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Encabezado de autorización JWT utilizando el esquema Portador. Ingrese 'Bearer' [Espacio] y luego su token en la entrada de texto a continuación",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                    }
                });
            });
            Services.AddAutoMapper(typeof(Program).Assembly); // Or use your startup class

            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IRoleRepository, RoleRepository>();
            Services.AddScoped<IFileRepository, FileRepository>();
            Services.AddScoped<IAuthRepository, AuthRepository>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IStockHistoryRepository, StockHistoryRepository>();
            Services.AddScoped<StockService>();

            Services.AddScoped<AuthService>();
            Services.AddScoped<FileService>();
            Services.AddScoped<ProductService>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly("API")));
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = configuration["Jwt:Issuer"],
                       ValidAudience = configuration["Jwt:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                       ClockSkew = TimeSpan.Zero

                   };
            });

            Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 5;           // 5 intentos permitidos
                    options.Window = TimeSpan.FromMinutes(1);  // Por minuto
                    options.QueueLimit = 0;            // Importante: ponemos esto en 0 para que no haga cola
                    options.AutoReplenishment = true;  // Se resetea automáticamente después del tiempo
                });

                // Manejador de rechazo mejorado
                options.OnRejected = async (context, _) =>
                {
                    context.HttpContext.Response.StatusCode = 429; // Too Many Requests
                    context.HttpContext.Response.ContentType = "application/json";

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Has excedido el límite de intentos permitidos. Por favor, espera un momento antes de intentar nuevamente.",
                        ErrorCode = "RATE_LIMIT_EXCEEDED"
                    };

                    // Calcular tiempo restante
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        response.Data = new
                        {
                            RetryAfterSeconds = (int)retryAfter.TotalSeconds,
                            Message = $"Podrás intentar nuevamente en {(int)retryAfter.TotalSeconds} segundos"
                        };

                        // Agregar header de retry-after
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString();
                    }

                    await context.HttpContext.Response.WriteAsJsonAsync(response);
                };

                // Configuración global
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });
            Services.Configure<FileSettings>(configuration.GetSection("FileStorage"));

            // Obtener la configuración de manera segura
            var fileSettings = new FileSettings();
            configuration.GetSection("FileStorage").Bind(fileSettings);

            // Configurar FormOptions con los valores obtenidos
            Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = fileSettings.MaxFileSize;
                options.ValueLengthLimit = int.MaxValue;
            });

            // Si necesitas acceder a FileSettings en otras partes de la aplicación
            Services.AddSingleton(fileSettings);

            return Services;

        }

    }
}
