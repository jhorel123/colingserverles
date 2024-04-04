using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Coling.Utilitarios.Attributes;
using Coling.Utilitarios.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarAfiliadoIdiomas", "ListarAfiliadoIdiomas", Description = "Lista todos los idiomas de afiliados.")]
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarAfiliadoIdioma", "InsertarAfiliadoIdioma", Description = "Inserta un nuevo idioma de afiliado.")]
        [OpenApiRequestBody("application/json", typeof(AfiliadoIdioma), Description = "Datos del idioma del afiliado a insertar")]
        public async Task<HttpResponseData> InsertarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliadoidioma")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar afiliado idioma.");
            try
            {
                var afiliadoIdioma = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un idioma de afiliado con todos sus datos");
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("ModificarAfiliadoIdioma", "ModificarAfiliadoIdioma", Description = "Modifica un idioma de afiliado existente.")]
        [OpenApiRequestBody("application/json", typeof(AfiliadoIdioma), Description = "Datos del idioma del afiliado a modificar")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del idioma de afiliado", Description = "El ID del idioma del afiliado a modificar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ModificarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliadoidioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar idioma de afiliado con Id: {id}.");
            try
            {
                var afiliadoIdioma = await req.ReadFromJsonAsync<AfiliadoIdioma>() ?? throw new Exception("Debe ingresar un idioma de afiliado con todos sus datos");
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EliminarAfiliadoIdioma", "EliminarAfiliadoIdioma", Description = "Elimina un idioma de afiliado existente.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del idioma de afiliado", Description = "El ID del idioma de afiliado a eliminar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EliminarAfiliadoIdioma(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliadoidioma/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar idioma de afiliado con Id: {id}.");
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ObtenerAfiliadoIdiomaById", "ObtenerAfiliadoIdiomaById", Description = "Obtiene un afiliado idioma por su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del afiliado idioma", Description = "El ID del afiliado idioma a obtener", Visibility = OpenApiVisibilityType.Important)]
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