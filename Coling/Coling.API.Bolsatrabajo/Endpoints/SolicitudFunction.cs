using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using Coling.Utilitarios.Attributes;
using Coling.Utilitarios.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.EndPoints
{
    public class SolicitudFunction
    {
        private readonly ILogger<SolicitudFunction> _logger;
        private readonly ISolicitudLogic _solicitudRepository;

        public SolicitudFunction(ILogger<SolicitudFunction> logger, ISolicitudLogic solicitudRepository)
        {
            _logger = logger;
            _solicitudRepository = solicitudRepository;
        }

        [Function("InsertarSolicitud")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarSolicitud", "Inserta una solicitud", Summary = "Inserta una solicitud en el sistema.")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Objeto de tipo Solicitud que representa la solicitud a insertar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de inserción exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> InsertarSolicitud(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                var solicitud = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una solicitud con todos sus datos");
                bool success = await _solicitudRepository.InsertarSolicitud(solicitud);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar solicitud");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ListarSolicitudes")]

        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarSolicitudes", "Lista todas las solicitudes", Summary = "Lista todas las solicitudes registradas en el sistema.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Solicitud>), Summary = "Operación exitosa", Description = "Operación de listado exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Summary = "Error interno", Description = "Se produjo un error interno al procesar la solicitud.")]
        public async Task<IActionResult> ListarSolicitudes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                var listaSolicitudes = await _solicitudRepository.ListarSolicitudes();
                return new OkObjectResult(listaSolicitudes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar solicitudes");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ModificarSolicitud")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("ModificarSolicitud", "Modifica una solicitud", Summary = "Modifica una solicitud existente en el sistema.")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Objeto de tipo Solicitud que representa la solicitud a modificar.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la solicitud", Description = "El ID de la solicitud a modificar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de modificación exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> ModificarSolicitud(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarSolicitud/{id}")] HttpRequestData req,
            string id)
        {
            try
            {
                var solicitud = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una solicitud con todos sus datos");
                bool success = await _solicitudRepository.ModificarSolicitud(solicitud, id);
                return success ? new OkResult() : new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar solicitud");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("EliminarSolicitud")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EliminarSolicitud", "Elimina una solicitud", Summary = "Elimina una solicitud del sistema.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la solicitud", Description = "El ID de la solicitud a eliminar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de eliminación exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> EliminarSolicitud(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarSolicitud/{id}")] HttpRequestData req,
    string id)
        {
            try
            {
                bool success = await _solicitudRepository.EliminarSolicitud(id);
                return success ? new OkResult() : new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar solicitud");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ObtenerSolicitudById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ObtenerSolicitudById", "Obtiene una solicitud por su ID", Summary = "Obtiene una solicitud del sistema mediante su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la solicitud", Description = "El ID de la solicitud a obtener.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Summary = "Operación exitosa", Description = "Operación de obtención exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "No encontrado", Description = "La solicitud con el ID proporcionado no fue encontrada.")]
        public async Task<IActionResult> ObtenerSolicitudById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerSolicitudById/{id}")] HttpRequestData req,
            string id)
        {
            try
            {
                var solicitud = await _solicitudRepository.ObtenerSolicitudById(id);
                return solicitud != null ? new OkObjectResult(solicitud) : new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitud por ID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
