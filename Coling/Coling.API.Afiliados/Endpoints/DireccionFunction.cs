using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

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
