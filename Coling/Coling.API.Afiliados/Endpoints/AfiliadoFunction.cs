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
    public class AfiliadoFunction
    {
        private readonly ILogger<AfiliadoFunction> _logger;
        private readonly IAfiliadoLogic afiliadoLogic;

        public AfiliadoFunction(ILogger<AfiliadoFunction> logger, IAfiliadoLogic afiliadoLogic)
        {
            _logger = logger;
            this.afiliadoLogic = afiliadoLogic;
        }

        [Function("ListarAfiliados")]
        public async Task<HttpResponseData> ListarAfiliados(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar afiliados.");
            try
            {
                var listaAfiliados = await afiliadoLogic.ListarAfiliados();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliados);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarAfiliado")]
        public async Task<HttpResponseData> InsertarAfiliado(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar afiliado.");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seGuardo = await afiliadoLogic.InsertarAfiliado(afiliado);

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

        [Function("ModificarAfiliado")]
        public async Task<HttpResponseData> ModificarAfiliado(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliado/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar afiliado con Id: {id}.");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seModifico = await afiliadoLogic.ModificarAfiliado(afiliado, id);

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

        [Function("EliminarAfiliado")]
        public async Task<HttpResponseData> EliminarAfiliado(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliado/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar afiliado con Id: {id}.");
            try
            {
                bool seElimino = await afiliadoLogic.EliminarAfiliado(id);

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

        [Function("ObtenerAfiliadoById")]
        public async Task<HttpResponseData> ObtenerAfiliadoById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerafiliadobyid/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener afiliado con Id: {id}.");
            try
            {
                var afiliado = await afiliadoLogic.ObtenerAfiliadoById(id);

                if (afiliado != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(afiliado);
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
