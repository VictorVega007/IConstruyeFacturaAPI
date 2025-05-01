using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IConstruye.Factura.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider  _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Factura API",
            Version = description.ApiVersion.ToString(),
            Description = "API Factura gestión facturación usuario",
            Contact = new OpenApiContact() { Name = "IConstruye", Email = "victorvega.v@gmail.com" }
        };

        if (description.IsDeprecated)
        {
            info.Description += "La versión de esta API está deprecada";
        }

        return info;

    }
}