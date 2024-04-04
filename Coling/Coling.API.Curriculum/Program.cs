
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coling.Utilitarios.Middlewares;
using System.Reflection;
using Coling.Utilitarios.Attributes;
using System.Runtime.CompilerServices;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var ensamblado = Assembly.GetExecutingAssembly();
var tipoAtributo = typeof(ColingAuthorizeAttribute);
IList<string> DefaultFunctions = new List<string>();
var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        //services.AddAutoMapper(ensamblado);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IGradoAcademicoRepositorio, GradoAcademicoRepositorio>();
        services.AddScoped<IProfesionRepositorio, ProfesionRepositorio>();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudioRepositorio, EstudioRepositorio>();
        services.AddScoped<ITipoEstudioRepositorio, TipoEstudioRepositorio>();
        services.AddScoped<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();
        services.AddSingleton<JwtMiddleware>();
        services.AddSingleton<AutorizacionRolMiddleware>();
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    }).ConfigureFunctionsWebApplication(x =>
    {
        x.UseMiddleware<JwtMiddleware>();
        x.UseMiddleware<AutorizacionRolMiddleware>();
    })
    .Build();
var lista = GetAuthorizedFunctions(ensamblado, tipoAtributo);
host.Services.GetService<AutorizacionRolMiddleware>()?.SetFunctionAutorizadas(lista);
host.Run();


Dictionary<string, string> GetAuthorizedFunctions2(Type containerType, Type? attributeType)
{
    var authorizedFunctions = new Dictionary<string, string>();

    if (attributeType == null) { return authorizedFunctions; }

    Type functionType = typeof(FunctionAttribute);
    var methods = ObtenerMetdosConAtributoPersonalizado(containerType, functionType);
    foreach (var method in methods)
    {
        var function = method.GetCustomAttribute(attributeType, true) as ColingAuthorizeAttribute;
        if (function is null)
        {
            continue;
        }

        var functionA = (FunctionAttribute?)method.GetCustomAttribute(functionType, true);
        if (functionA is null)
        {
            continue;
        }
        authorizedFunctions.Add(functionA.Name, function.Rols);
    }

    return authorizedFunctions;
}

MethodInfo[] ObtenerMetdosConAtributoPersonalizado(Type type, Type attributeType)
{
    return type.GetMethods()
        .Where(m => m.GetCustomAttributes(attributeType, inherit: true).Any())
        .ToArray();
}

Dictionary<string, string> GetAuthorizedFunctions(Assembly containerAssembly, Type? attributeType)
{
    if (containerAssembly == null) throw new ArgumentNullException(nameof(containerAssembly));
    if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

    var result = new Dictionary<string, string>();
    foreach (var item in containerAssembly.ExportedTypes)
    {
        var resulItem = GetAuthorizedFunctions2(item, attributeType);
        result.AddRange(resulItem);
    }

    return result;
}