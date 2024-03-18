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
    public class TelefonoFunction
    {
        private readonly ILogger<TelefonoFunction> _logger;
        private readonly ITelefonoLogic telefonoLogic;

        public TelefonoFunction(ILogger<TelefonoFunction> logger, ITelefonoLogic telefonoLogic)
        {
            _logger = logger;
            this.telefonoLogic = telefonoLogic;
        }

        [Function("ListarTelefonosPorPersona")]
        public async Task<HttpResponseData> ListarTelefonosPorPersona(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartelefonosporpersona/{idPersona}")] HttpRequestData req,
            int idPersona)
        {
            _logger.LogInformation($"Ejecutando azure function para listar tel�fonos de la persona con Id: {idPersona}.");
            try
            {
                var listaTelefonos = await telefonoLogic.ListarTelefonosPorPersona(idPersona);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTelefonos);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarTelefono")]
        public async Task<HttpResponseData> InsertarTelefono([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertartelefono")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar tel�fono.");
            try
            {
                var telefono = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un tel�fono con todos sus datos");
                bool seGuardo = await telefonoLogic.InsertarTelefono(telefono);

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

        [Function("ModificarTelefono")]
        public async Task<HttpResponseData> ModificarTelefono(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificartelefono/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar tel�fono con Id: {id}.");
            try
            {
                var telefono = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un tel�fono con todos sus datos");
                bool seModifico = await telefonoLogic.ModificarTelefono(telefono, id);

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

        [Function("EliminarTelefono")]
        public async Task<HttpResponseData> EliminarTelefono(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminartelefono/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar tel�fono con Id: {id}.");
            try
            {
                bool seElimino = await telefonoLogic.EliminarTelefono(id);

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

        [Function("ObtenerTelefonoById")]
        public async Task<HttpResponseData> ObtenerTelefonoById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenertelefonobyid/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener tel�fono con Id: {id}.");
            try
            {
                var telefono = await telefonoLogic.ObtenerTelefonoById(id);

                if (telefono != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(telefono);
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
