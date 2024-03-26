using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
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
    public class EstudioFunction
    {
        private readonly ILogger<EstudioFunction> _logger;
        private readonly IEstudioRepositorio repositorio;

        public EstudioFunction(ILogger<EstudioFunction> logger, IEstudioRepositorio repositorio)
        {
            _logger = logger;
            this.repositorio = repositorio;
        }

        [Function("InsertarEstudio")]
        [OpenApiOperation("InsertarEstudio", "InsertarEstudio", Description = "Sirve para ingresar un estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudio), Description = "Ingresar estudio nuevo")]
        public async Task<HttpResponseData> InsertarEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var estudio = await req.ReadFromJsonAsync<Estudio>() ?? throw new Exception("Debe ingresar un estudio con todos sus datos");
                estudio.RowKey = Guid.NewGuid().ToString();
                estudio.Timestamp = DateTimeOffset.UtcNow;
                bool success = await repositorio.Create(estudio);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarEstudios")]
        [OpenApiOperation("ListarEstudios", "ListarEstudios", Description = "Sirve para listar todos los estudios")]
        public async Task<HttpResponseData> ListarEstudios(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var listaEstudios = await repositorio.GetAll();
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(listaEstudios);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("EditarEstudio")]
        [OpenApiOperation("EditarEstudio", "EditarEstudio", Description = "Sirve para editar un estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudio), Description = "Editar estudio")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID del estudio", Description = "El RowKey del estudio a editar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EditarEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarEstudio/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var estudio = await req.ReadFromJsonAsync<Estudio>() ?? throw new Exception("Debe ingresar un estudio con todos sus datos");
                estudio.RowKey = rowKey;
                bool success = await repositorio.Update(estudio);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("BorrarEstudio")]
        [OpenApiOperation("BorrarEstudio", "BorrarEstudio", Description = "Sirve para eliminar un estudio")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey del estudio", Description = "El PartitionKey del estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey del estudio", Description = "El RowKey del estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> BorrarEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarEstudio/{partitionKey}/{rowKey}")] HttpRequestData req,
            string partitionKey, string rowKey)
        {
            HttpResponseData response;
            try
            {
                bool success = await repositorio.Delete(partitionKey, rowKey);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarEstudioById")]
        [OpenApiOperation("ListarEstudioById", "ListarEstudioById", Description = "Sirve para obtener un estudio por su ID")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID del estudio", Description = "El RowKey del estudio a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarEstudioById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarEstudioById/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var estudio = await repositorio.Get(rowKey);
                response = req.CreateResponse(estudio != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                if (estudio != null)
                {
                    await response.WriteAsJsonAsync(estudio);
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
