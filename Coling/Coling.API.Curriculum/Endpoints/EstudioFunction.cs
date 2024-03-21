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
        public async Task<HttpResponseData> EditarEstudio(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarEstudio/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var estudio = await req.ReadFromJsonAsync<Estudio>() ?? throw new Exception("Debe ingresar un estudio con todos sus datos");
                estudio.RowKey = id;
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
        public async Task<HttpResponseData> ListarEstudioById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarEstudioById/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var estudio = await repositorio.Get(id);
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
