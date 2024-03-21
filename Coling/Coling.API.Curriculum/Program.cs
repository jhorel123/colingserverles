using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IGradoAcademicoRepositorio, GradoAcademicoRepositorio>();
        services.AddScoped<IProfesionRepositorio, ProfesionRepositorio>();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudioRepositorio, EstudioRepositorio>();
        services.AddScoped<ITipoEstudioRepositorio, TipoEstudioRepositorio>();
        services.AddScoped<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();
        

    })
    .Build();

host.Run();
