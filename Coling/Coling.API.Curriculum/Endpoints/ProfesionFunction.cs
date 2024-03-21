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
    public class ProfesionFunction
    {
        private readonly ILogger<ProfesionFunction> _logger;
        private readonly IProfesionRepositorio repository;

        public ProfesionFunction(ILogger<ProfesionFunction> logger, IProfesionRepositorio repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [Function("InsertarProfesion")]
        public async Task<HttpResponseData> InsertarProfesion(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una profesión con todos sus datos");
                profesion.RowKey = Guid.NewGuid().ToString();
                profesion.Timestamp = DateTimeOffset.UtcNow;
                bool success = await repository.Create(profesion);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarProfesiones")]
        public async Task<HttpResponseData> ListarProfesiones(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var listaProfesiones = await repository.GetAll();
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(listaProfesiones);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("EditarProfesion")]
        public async Task<HttpResponseData> EditarProfesion(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarProfesion/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una profesión con todos sus datos");
                profesion.RowKey = id;
                bool success = await repository.Update(profesion);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("BorrarProfesion")]
        public async Task<HttpResponseData> BorrarProfesion(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarProfesion/{partitionKey}/{rowKey}")] HttpRequestData req,
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

        [Function("ListarProfesionById")]
        public async Task<HttpResponseData> ListarProfesionById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarProfesionById/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var profesion = await repository.Get(id);
                response = req.CreateResponse(profesion != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                if (profesion != null)
                {
                    await response.WriteAsJsonAsync(profesion);
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
