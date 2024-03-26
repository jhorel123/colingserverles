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

namespace Coling.API.Afiliados.Endpoints
{
    public class IdiomaFunction
    {
        private readonly ILogger<IdiomaFunction> _logger;
        private readonly IIdiomaLogic idiomaLogic;

        public IdiomaFunction(ILogger<IdiomaFunction> logger, IIdiomaLogic idiomaLogic)
        {
            _logger = logger;
            this.idiomaLogic = idiomaLogic;
        }

        [Function("ListarIdiomas")]
        [OpenApiOperation("ListarIdiomas", "ListarIdiomas", Description = "Lista todos los idiomas.")]
        public async Task<HttpResponseData> ListarIdiomas(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listaridiomas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar idiomas.");
            try
            {
                var listaIdiomas = await idiomaLogic.ListarIdiomas();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaIdiomas);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarIdioma")]
        [OpenApiOperation("InsertarIdioma", "InsertarIdioma", Description = "Inserta un nuevo idioma.")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Datos del idioma a insertar")]
        public async Task<HttpResponseData> InsertarIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertaridioma")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar idioma.");
            try
            {
                var idioma = await req.ReadFromJsonAsync<Idioma>() ?? throw new Exception("Debe ingresar un idioma con todos sus datos");
                bool seGuardo = await idiomaLogic.InsertarIdioma(idioma);

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

        [Function("ModificarIdioma")]
        [OpenApiOperation("ModificarIdioma", "ModificarIdioma", Description = "Modifica un idioma existente.")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Datos del idioma a modificar")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del idioma", Description = "El ID del idioma a modificar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ModificarIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificaridioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar idioma con Id: {id}.");
            try
            {
                var idioma = await req.ReadFromJsonAsync<Idioma>() ?? throw new Exception("Debe ingresar un idioma con todos sus datos");
                bool seModifico = await idiomaLogic.ModificarIdioma(idioma, id);

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

        [Function("EliminarIdioma")]
        [OpenApiOperation("EliminarIdioma", "EliminarIdioma", Description = "Elimina un idioma existente.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del idioma", Description = "El ID del idioma a eliminar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EliminarIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminaridioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar idioma con Id: {id}.");
            try
            {
                bool seElimino = await idiomaLogic.EliminarIdioma(id);

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

        [Function("ObtenerIdiomaById")]
        [OpenApiOperation("ObtenerIdiomaById", "ObtenerIdiomaById", Description = "Obtiene un idioma por su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del idioma", Description = "El ID del idioma a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ObtenerIdiomaById(
     [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obteneridiomabyid/{id}")] HttpRequestData req,
     int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener idioma con Id: {id}.");
            try
            {
                var idioma = await idiomaLogic.ObtenerIdiomaById(id);

                if (idioma != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(idioma);
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