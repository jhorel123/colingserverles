using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> EditarTipoEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarTipoEstudio/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var tipoEstudio = await req.ReadFromJsonAsync<TipoEstudio>() ?? throw new Exception("Debe ingresar un tipo de estudio con todos sus datos");
                tipoEstudio.RowKey = id;
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
        public async Task<HttpResponseData> ListarTipoEstudioById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarTipoEstudioById/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var tipoEstudio = await repository.Get(id);
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
