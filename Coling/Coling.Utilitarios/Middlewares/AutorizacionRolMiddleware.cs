using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Utilitarios.Middlewares
{
    public class AutorizacionRolMiddleware : IFunctionsWorkerMiddleware
    {
        Dictionary<string, string> funcionesRolesAutorizados = new Dictionary<string, string>();
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var solicitud = await context.GetHttpRequestDataAsync();
            if (solicitud is null) return;

            var nombreFunction = solicitud.FunctionContext.FunctionDefinition.Name;
            var rolesClaim = (string?)context.Items["rolesclaim"];
            if (!EstaAutorizado(nombreFunction, rolesClaim))
            {
                throw new Exception("Error: No esta autorizado");
            }

            await next(context);

        }
        //"Admin,Afiliado"
        private bool EstaAutorizado(string nombreFuncion, string? rolesClaim)
        {
            var rolesClaims = funcionesRolesAutorizados.TryGetValue(nombreFuncion, out string? rolesString) &&
                rolesString.Split(',').Contains(rolesClaim) ? true : false;

            return rolesClaims;
        }

        public void SetFunctionAutorizadas(Dictionary<string, string> metodosAutorizados)
        {
            if (metodosAutorizados != null)
            {
                foreach (var item in metodosAutorizados)
                {
                    funcionesRolesAutorizados.Add(item.Key, item.Value);
                }
            }
        }
    }
}