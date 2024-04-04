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
    public class PersonaTipoSocialFunction
    {
        private readonly ILogger<PersonaTipoSocialFunction> _logger;
        private readonly IPersonaTipoSocialLogic personaTipoSocialLogic;

        public PersonaTipoSocialFunction(ILogger<PersonaTipoSocialFunction> logger, IPersonaTipoSocialLogic personaTipoSocialLogic)
        {
            _logger = logger;
            this.personaTipoSocialLogic = personaTipoSocialLogic;
        }

        [Function("ListarPersonaTiposSocial")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarPersonaTiposSocial", "ListarPersonaTiposSocial", Description = "Lista los tipos de social de las personas.")]
        public async Task<HttpResponseData> ListarPersonaTiposSocial(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarpersonatipossocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar persona tipos social.");
            try
            {
                var listaPersonaTiposSocial = await personaTipoSocialLogic.ListarPersonaTiposSocial();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonaTiposSocial);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarPersonaTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarPersonaTipoSocial", "InsertarPersonaTipoSocial", Description = "Inserta un nuevo tipo de social de persona.")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarpersonatiposocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar persona tipo social.");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un persona tipo social con todos sus datos");
                bool seGuardo = await personaTipoSocialLogic.InsertarPersonaTipoSocial(personaTipoSocial);

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

        [Function("ModificarPersonaTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("ModificarPersonaTipoSocial", "ModificarPersonaTipoSocial", Description = "Modifica un tipo de social de persona existente.")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarpersonatiposocial/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar persona tipo social con Id: {id}.");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un persona tipo social con todos sus datos");
                bool seModifico = await personaTipoSocialLogic.ModificarPersonaTipoSocial(personaTipoSocial, id);

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

        [Function("EliminarPersonaTipoSocial")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EliminarPersonaTipoSocial", "EliminarPersonaTipoSocial", Description = "Elimina un tipo de social de persona existente.")]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarpersonatiposocial/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar persona tipo social con Id: {id}.");
            try
            {
                bool seElimino = await personaTipoSocialLogic.EliminarPersonaTipoSocial(id);

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

        [Function("ObtenerPersonaTipoSocialById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ObtenerPersonaTipoSocialById", "ObtenerPersonaTipoSocialById", Description = "Obtiene un tipo de social de persona por su ID.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del tipo de social de persona", Description = "El ID del tipo de social de persona a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ObtenerPersonaTipoSocialById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerpersonatiposocialbyid/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"Ejecutando azure function para obtener persona tipo social con Id: {id}.");
            try
            {
                var personaTipoSocial = await personaTipoSocialLogic.ObtenerPersonaTipoSocialById(id);

                if (personaTipoSocial != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(personaTipoSocial);
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
