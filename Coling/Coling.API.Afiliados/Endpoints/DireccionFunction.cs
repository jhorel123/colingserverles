using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Coling.Utilitarios.Attributes;
using Coling.Utilitarios.Roles;

namespace Coling.API.Afiliados.Endpoints
{
    public class DireccionFunction
    {
        private readonly ILogger<DireccionFunction> _logger;
        private readonly IDireccionLogic direccionLogic;

        public DireccionFunction(ILogger<DireccionFunction> logger, IDireccionLogic direccionLogic)
        {
            _logger = logger;
            this.direccionLogic = direccionLogic;
        }

        [Function("ListarDireccionesPorPersona")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarDireccionesPorPersona", "ListarDireccionesPorPersona", Description = "Lista las direcciones de una persona por su ID.")]
        [OpenApiParameter(name: "idPersona", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID de la persona", Description = "El ID de la persona cuyas direcciones se van a listar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarDireccionesPorPersona(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listardireccionesporpersona/{idPersona}")] HttpRequestData req,
             int idPersona)
        {
            _logger.LogInformation($"Ejecutando azure function para listar direcciones de la persona con Id: {idPersona}.");
            try
            {
                var listaDirecciones = await direccionLogic.ListarDireccionesPorPersona(idPersona);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaDirecciones);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarDireccion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarDireccion", "InsertarDireccion", Description = "Inserta una nueva dirección.")]
        [OpenApiRequestBody("application/json", typeof(Direccion), Description = "Datos de la dirección a insertar")]
        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertardireccion")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar dirección.");
            try
            {
                var direccion = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una dirección con todos sus datos");
                bool seGuardo = await direccionLogic.InsertarDireccion(direccion);

                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("ModificarDireccion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("ModificarDireccion", "ModificarDireccion", Description = "Modifica una dirección existente.")]
        [OpenApiRequestBody("application/json", typeof(Direccion), Description = "Datos de la dirección a modificar")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID de la dirección", Description = "El ID de la dirección a modificar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ModificarDireccion(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificardireccion/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar dirección con Id: {id}.");
            try
            {
                var direccion = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una dirección con todos sus datos");
                bool seModifico = await direccionLogic.ModificarDireccion(direccion, id);

                if (seModifico)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("EliminarDireccion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EliminarDireccion", "EliminarDireccion", Description = "Elimina una dirección existente.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID de la dirección", Description = "El ID de la dirección a eliminar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EliminarDireccion(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminardireccion/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar dirección con Id: {id}.");
            try
            {
                bool seElimino = await direccionLogic.EliminarDireccion(id);

                if (seElimino)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("ObtenerDireccionById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ObtenerDireccionById", "ObtenerDireccionById", Description = "Obtiene una dirección por su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID de la dirección", Description = "El ID de la dirección a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ObtenerDireccionById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerdireccionbyid/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener dirección con Id: {id}.");
            try
            {
                var direccion = await direccionLogic.ObtenerDireccionById(id);

                if (direccion != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(direccion);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
    }
}
