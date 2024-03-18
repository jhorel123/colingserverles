using Coling.API.Afiliados;
using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddDbContext<Contexto>(options => options.UseSqlServer(
                     configuration.GetConnectionString("cadenaConexion")), ServiceLifetime.Scoped);

        services.AddScoped<IAfiliadoLogic, AfiliadoLogic>();
        services.AddScoped<IIdiomaLogic, IdiomaLogic>();
        services.AddScoped<IAfiliadoIdiomaLogic, AfiliadoIdiomaLogic>();
        services.AddScoped<IPersonaLogic, PersonaLogic>();
        services.AddScoped<IDireccionLogic, DireccionLogic>();
        services.AddScoped<ITelefonoLogic, TelefonoLogic>();
        services.AddScoped<ITipoSocialLogic, TipoSocialLogic>();
        services.AddScoped<IPersonaTipoSocialLogic, PersonaTipoSocialLogic>();
    })
    .Build();

host.Run();
