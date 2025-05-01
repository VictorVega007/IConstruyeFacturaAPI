using System.Reflection;
using System.Text;
using IConstruye.Application.Handlers;
using IConstruye.Application.Mappers;
using IConstruye.Factura.Core.Interfaces;
using IConstruye.Factura.Core.Repositories;
using IConstruye.Factura.Core.Repositories.Base;
using IConstruye.Factura.Core.Services;
using IConstruye.Factura.Infrastructure.Data;
using IConstruye.Factura.Infrastructure.Helpers;
using IConstruye.Factura.Infrastructure.Repositories;
using IConstruye.Factura.Infrastructure.Repositories.Base;
using IConstruye.Factura.Infrastructure.Validators;
using IConstruye.Factura.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IConstruye.Factura;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Ingresa el token como: Bearer {tu_token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            
        });
        
        services.AddDbContext<InvoiceContext>(
            m => m.UseSqlServer(Configuration.GetConnectionString("InvoiceConnectionString")), ServiceLifetime.Singleton);
        
        services.AddAutoMapper(typeof(InvoiceMappingProfile).GetTypeInfo().Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UploadInvoiceCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GetInvoiceByUrlQueryHandler).Assembly);
        });
        
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IXmlInvoiceValidator, XmlInvoiceValidator>();
        services.AddSingleton<IInvoiceUrlService, InvoiceUrlService>();
        services.AddSingleton<ICertificateProvider, CertificateProvider>();

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("BearerToken", options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = Configuration.GetSection("TokenSettings")["Issuer"],
                ValidAudience = Configuration.GetSection("TokenSettings")["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TokenSettings")["ClaveSecreta"]!)),
                ClockSkew = TimeSpan.FromSeconds(0)
            };
        });
        
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("BearerToken")
                .Build();
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
        app.UseHttpsRedirection();
        app.UseDeveloperExceptionPage();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        
    }
}