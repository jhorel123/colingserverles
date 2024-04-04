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
    public class TipoSocialFunction
    {
        private readonly ILogger<TipoSocialFunction> _logger;
        private readonly ITipoSocialLogic tipoSocialLogic;

        public TipoSocialFunction(ILogger<TipoSocialFunction> logger, ITipoSocialLogic tipoSocialLogic)
        {
            _logger = logger;
            this.tipoSocialLogic = tipoSocialLogic;
        }

        [Function("ListarTiposSocial")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarTiposSocial", "ListarTiposSocial", Description = "Obtiene una lista de tipos sociales.")]
        public async Task<HttpResponseData> ListarTiposSocial(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartipossocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar tipos social.");
            try
            {
                var listaTiposSocial = await tipoSocialLogic.ListarTiposSocial();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTiposSocial);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarTipoSocial", "InsertarTipoSocial", Description = "Inserta un nuevo tipo social.")]
        public async Task<HttpResponseData> InsertarTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertartiposocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar tipo social.");
            try
            {
                var tipoSocial = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un tipo social con todos sus datos");
                bool seGuardo = await tipoSocialLogic.InsertarTipoSocial(tipoSocial);

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

        [Function("ModificarTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("ModificarTipoSocial", "ModificarTipoSocial", Description = "Modifica un tipo social existente.")]
        public async Task<HttpResponseData> ModificarTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificartiposocial/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar tipo social con Id: {id}.");
            try
            {
                var tipoSocial = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un tipo social con todos sus datos");
                bool seModifico = await tipoSocialLogic.ModificarTipoSocial(tipoSocial, id);

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

        [Function("EliminarTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EliminarTipoSocial", "EliminarTipoSocial", Description = "Elimina un tipo social existente.")]
        public async Task<HttpResponseData> EliminarTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminartiposocial/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar tipo social con Id: {id}.");
            try
            {
                bool seElimino = await tipoSocialLogic.EliminarTipoSocial(id);

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

        [Function("ObtenerTipoSocialById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ObtenerTipoSocialById", "ObtenerTipoSocialById", Description = "Obtiene un tipo social por su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del tipo social", Description = "El ID del tipo social a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ObtenerTipoSocialById(
     [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenertiposocialbyid/{id}")] HttpRequestData req,
     int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener tipo social con Id: {id}.");
            try
            {
                var tipoSocial = await tipoSocialLogic.ObtenerTipoSocialById(id);

                if (tipoSocial != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(tipoSocial);
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
