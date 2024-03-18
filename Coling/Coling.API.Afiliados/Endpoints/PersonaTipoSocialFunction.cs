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
