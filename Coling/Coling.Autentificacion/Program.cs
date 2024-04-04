using Coling.Repositorio.Contratos;
using Coling.Repositorio.Implementacion;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

    })
    .Build();

host.Run();
