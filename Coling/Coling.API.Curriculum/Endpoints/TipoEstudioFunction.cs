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
using System;
using System.Net;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.EndPoints
{
    public class TipoEstudioFunction
    {
        private readonly ILogger<TipoEstudioFunction> _logger;
        private readonly ITipoEstudioRepositorio repository;

        public TipoEstudioFunction(ILogger<TipoEstudioFunction> logger, ITipoEstudioRepositorio repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [Function("InsertarTipoEstudio")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarTipoEstudio", "InsertarTipoEstudio", Description = "Sirve para ingresar un tipo de estudio")]
        [OpenApiRequestBody("application/json", typeof(TipoEstudio), Description = "Ingresar tipo de estudio nuevo")]
        public async Task<HttpResponseData> InsertarTipoEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var tipoEstudio = await req.ReadFromJsonAsync<TipoEstudio>() ?? throw new Exception("Debe ingresar un tipo de estudio con todos sus datos");
                tipoEstudio.RowKey = Guid.NewGuid().ToString();
                tipoEstudio.Timestamp = DateTimeOffset.UtcNow;
                bool success = await repository.Create(tipoEstudio);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarTipoEstudios")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarTipoEstudios", "ListarTipoEstudios", Description = "Sirve para listar todos los tipos de estudios")]
        public async Task<HttpResponseData> ListarTipoEstudios(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var listaTipoEstudios = await repository.GetAll();
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(listaTipoEstudios);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("EditarTipoEstudio")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EditarTipoEstudio", "EditarTipoEstudio", Description = "Sirve para editar un tipo de estudio")]
        [OpenApiRequestBody("application/json", typeof(TipoEstudio), Description = "Editar tipo de estudio")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID del tipo de estudio", Description = "El RowKey del tipo de estudio a editar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EditarTipoEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarTipoEstudio/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var tipoEstudio = await req.ReadFromJsonAsync<TipoEstudio>() ?? throw new Exception("Debe ingresar un tipo de estudio con todos sus datos");
                tipoEstudio.RowKey = rowKey;
                bool success = await repository.Update(tipoEstudio);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("BorrarTipoEstudio")]
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("BorrarTipoEstudio", "BorrarTipoEstudio", Description = "Sirve para eliminar un tipo de estudio")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey del tipo de estudio", Description = "El PartitionKey del tipo de estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey del tipo de estudio", Description = "El RowKey del tipo de estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> BorrarTipoEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarTipoEstudio/{partitionKey}/{rowKey}")] HttpRequestData req,
            string partitionKey, string rowKey)
        {
            HttpResponseData response;
            try
            {
                bool success = await repository.Delete(partitionKey, rowKey);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarTipoEstudioById")]
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarTipoEstudioById", "ListarTipoEstudioById", Description = "Sirve para obtener un tipo de estudio por su ID")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID del tipo de estudio", Description = "El RowKey del tipo de estudio a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarTipoEstudioById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarTipoEstudioById/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var tipoEstudio = await repository.Get(rowKey);
                response = req.CreateResponse(tipoEstudio != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                if (tipoEstudio != null)
                {
                    await response.WriteAsJsonAsync(tipoEstudio);
                }
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
    }
}
