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
    public class AfiliadoIdiomaFunction
    {
        private readonly ILogger<AfiliadoIdiomaFunction> _logger;
        private readonly IAfiliadoIdiomaLogic afiliadoIdiomaLogic;

        public AfiliadoIdiomaFunction(ILogger<AfiliadoIdiomaFunction> logger, IAfiliadoIdiomaLogic afiliadoIdiomaLogic)
        {
            _logger = logger;
            this.afiliadoIdiomaLogic = afiliadoIdiomaLogic;
        }

        [Function("ListarAfiliadoIdiomas")]
        public async Task<HttpResponseData> ListarAfiliadoIdiomas(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliadoidiomas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar afiliado idiomas.");
            try
            {
                var listaAfiliadoIdiomas = await afiliadoIdiomaLogic.ListarAfiliadoIdiomas();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliadoIdiomas);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarAfiliadoIdioma")]
        public async Task<HttpResponseData> InsertarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliadoidioma")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar afiliado idioma.");
            try
            {
                var afiliadoIdioma = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un afiliado idioma con todos sus datos");
                bool seGuardo = await afiliadoIdiomaLogic.InsertarAfiliadoIdioma(afiliadoIdioma);

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

        [Function("ModificarAfiliadoIdioma")]
        public async Task<HttpResponseData> ModificarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliadoidioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar afiliado idioma con Id: {id}.");
            try
            {
                var afiliadoIdioma = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un afiliado idioma con todos sus datos");
                bool seModifico = await afiliadoIdiomaLogic.ModificarAfiliadoIdioma(afiliadoIdioma, id);

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

        [Function("EliminarAfiliadoIdioma")]
        public async Task<HttpResponseData> EliminarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliadoidioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar afiliado idioma con Id: {id}.");
            try
            {
                bool seElimino = await afiliadoIdiomaLogic.EliminarAfiliadoIdioma(id);

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

        [Function("ObtenerAfiliadoIdiomaById")]
        public async Task<HttpResponseData> ObtenerAfiliadoIdiomaById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerafiliadoidiomabyid/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener afiliado idioma con Id: {id}.");
            try
            {
                var afiliadoIdioma = await afiliadoIdiomaLogic.ObtenerAfiliadoIdiomaById(id);

                if (afiliadoIdioma != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(afiliadoIdioma);
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
