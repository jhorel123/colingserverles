using Coling.API.Bolsatrabajo.Contratos;
using Coling.API.Bolsatrabajo.Implementacion;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights(); 
        services.AddScoped<ISolicitudLogic, SolicitudLogic>();
        services.AddScoped<IOfertaLaboralLogic, OfertaLaboralLogic>();
    })
    .Build();

host.Run();
