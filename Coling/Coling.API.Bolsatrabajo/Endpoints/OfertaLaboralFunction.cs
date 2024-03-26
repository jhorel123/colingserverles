using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.EndPoints
{
    public class OfertaLaboralFunction
    {
        private readonly ILogger<OfertaLaboralFunction> _logger;
        private readonly IOfertaLaboralLogic _ofertaLaboralRepository;

        public OfertaLaboralFunction(ILogger<OfertaLaboralFunction> logger, IOfertaLaboralLogic ofertaLaboralRepository)
        {
            _logger = logger;
            _ofertaLaboralRepository = ofertaLaboralRepository;
        }

        [Function("InsertarOfertaLaboral")]
        [OpenApiOperation("InsertarOfertaLaboral", "Inserta una oferta laboral", Summary = "Inserta una oferta laboral en el sistema.")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboral), Description = "Objeto de tipo OfertaLaboral que representa la oferta laboral a insertar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de inserción exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> InsertarOfertaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                var ofertaLaboral = await req.ReadFromJsonAsync<OfertaLaboral>() ?? throw new Exception("Debe ingresar una oferta laboral con todos sus datos");
                bool success = await _ofertaLaboralRepository.InsertarOfertaLaboral(ofertaLaboral);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar oferta laboral");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ListarOfertasLaborales")]
        [OpenApiOperation("ListarOfertasLaborales", "Lista todas las ofertas laborales", Summary = "Lista todas las ofertas laborales registradas en el sistema.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Summary = "Operación exitosa", Description = "Operación de listado exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Summary = "Error interno", Description = "Se produjo un error interno al procesar la solicitud.")]
        public async Task<IActionResult> ListarOfertasLaborales(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                var listaOfertas = await _ofertaLaboralRepository.ListarOfertasLaborales();
                return new OkObjectResult(listaOfertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar ofertas laborales");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ModificarOfertaLaboral")]
        [OpenApiOperation("ModificarOfertaLaboral", "Modifica una oferta laboral", Summary = "Modifica una oferta laboral existente en el sistema.")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboral), Description = "Objeto de tipo OfertaLaboral que representa la oferta laboral a modificar.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la oferta laboral", Description = "El ID de la oferta laboral a modificar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de modificación exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> ModificarOfertaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarOfertaLaboral/{id}")] HttpRequestData req,
            string id)
        {
            try
            {
                var ofertaLaboral = await req.ReadFromJsonAsync<OfertaLaboral>() ?? throw new Exception("Debe ingresar una oferta laboral con todos sus datos");
                bool success = await _ofertaLaboralRepository.ModificarOfertaLaboral(ofertaLaboral, id);
                return success ? new OkResult() : new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar oferta laboral");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("EliminarOfertaLaboral")]
        [OpenApiOperation("EliminarOfertaLaboral", "Elimina una oferta laboral", Summary = "Elimina una oferta laboral del sistema.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la oferta laboral", Description = "El ID de la oferta laboral a eliminar.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Operación exitosa", Description = "Operación de eliminación exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Solicitud incorrecta", Description = "La solicitud no se pudo procesar debido a un error en los datos proporcionados.")]
        public async Task<IActionResult> EliminarOfertaLaboral(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarOfertaLaboral/{id}")] HttpRequestData req,
    string id)
        {
            try
            {
                bool success = await _ofertaLaboralRepository.EliminarOfertaLaboral(id);
                return success ? new OkResult() : new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar oferta laboral");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Function("ObtenerOfertaLaboralById")]
        [OpenApiOperation("ObtenerOfertaLaboralById", "Obtiene una oferta laboral por su ID", Summary = "Obtiene una oferta laboral del sistema mediante su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la oferta laboral", Description = "El ID de la oferta laboral a obtener.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Summary = "Operación exitosa", Description = "Operación de obtención exitosa.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "No encontrado", Description = "La oferta laboral con el ID proporcionado no fue encontrada.")]
        public async Task<IActionResult> ObtenerOfertaLaboralById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerOfertaLaboralById/{id}")] HttpRequestData req,
            string id)
        {
            try
            {
                var ofertaLaboral = await _ofertaLaboralRepository.ObtenerOfertaLaboralById(id);
                return ofertaLaboral != null ? new OkObjectResult(ofertaLaboral) : new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener oferta laboral por ID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

