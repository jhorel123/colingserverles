using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
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
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionRepositorio repos;

        public InstitucionFunction(ILogger<InstitucionFunction> logger, IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarInstitucion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("Insertarspec", "InsertarInstitucion", Description = " Sirve para ingresar una institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description = "Ingresar institucion nueva")]
        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una institucion con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Create(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ListarInstitucion")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("Listarspec", "ListarInstitucion", Description = " Sirve para listar todas las instituciones")]
        public async Task<HttpResponseData> ListarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EditarInstitucion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("Modificarspec", "EditarInstitucion", Description = " Sirve para editar una institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description = "editar institucion")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la Institución", Description = "El RowKey de la institución a editar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EditarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarInstitucion/{rowKey}")] HttpRequestData req, string rowKey)
        {
            HttpResponseData respuesta;
            try
            {
                var institucion = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una institucion con todos sus datos");
                institucion.RowKey = rowKey;
                bool sw = await repos.Update(institucion);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("BorrarInstitucion")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("Eliminarspec", "BorrarInstitucion", Description = " Sirve para eliminar una institucion")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la Institución", Description = "El PartitionKey de la institución a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la Institución", Description = "El RowKey de la institución a borrar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> BorrarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarInstitucion/{partitionKey}/{rowKey}")] HttpRequestData req, string partitionKey, string rowKey)
        {
            HttpResponseData respuesta;
            try
            {
                bool sw = await repos.Delete(partitionKey, rowKey);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }


        [Function("ListarInstitucionById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("Listaridspec", "ListarInstitucionById", Description = " Sirve para listar una institucion por id")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la Institución", Description = "El RowKey de la institución a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarInstitucionById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarInstitucionById/{rowKey}")] HttpRequestData req, string rowKey)
        {
            HttpResponseData respuesta;
            try
            {
                var institucion = await repos.Get(rowKey);
                if (institucion != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(institucion);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}