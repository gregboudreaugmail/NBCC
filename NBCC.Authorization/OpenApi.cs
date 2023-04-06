namespace NBCC.Authorization;

public sealed class OpenApi
{
    public static Action<SwaggerGenOptions> AddAuthentication()
    {
        return c =>
        {
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                   new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "basic"
                         }
                      },
                     Array.Empty<string>()
                }
            });
        };
    }
}
