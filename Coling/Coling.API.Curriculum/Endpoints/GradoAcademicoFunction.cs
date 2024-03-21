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
    public class GradoAcademicoFunction
    {
        private readonly ILogger<GradoAcademicoFunction> _logger;
        private readonly IGradoAcademicoRepositorio repository;

        public GradoAcademicoFunction(ILogger<GradoAcademicoFunction> logger, IGradoAcademicoRepositorio repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        [Function("InsertarGradoAcademico")]
        public async Task<HttpResponseData> InsertarGradoAcademico(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var gradoAcademico = await req.ReadFromJsonAsync<GradoAcademico>() ?? throw new Exception("Debe ingresar un grado académico con todos sus datos");
                gradoAcademico.RowKey = Guid.NewGuid().ToString();
                gradoAcademico.Timestamp = DateTimeOffset.UtcNow;
                bool success = await repository.Create(gradoAcademico);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarGradosAcademicos")]
        public async Task<HttpResponseData> ListarGradosAcademicos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var listaGradosAcademicos = await repository.GetAll();
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(listaGradosAcademicos);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("EditarGradoAcademico")]
        public async Task<HttpResponseData> EditarGradoAcademico(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarGradoAcademico/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var gradoAcademico = await req.ReadFromJsonAsync<GradoAcademico>() ?? throw new Exception("Debe ingresar un grado académico con todos sus datos");
                gradoAcademico.RowKey = id;
                bool success = await repository.Update(gradoAcademico);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("BorrarGradoAcademico")]
        public async Task<HttpResponseData> BorrarGradoAcademico(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarGradoAcademico/{partitionKey}/{rowKey}")] HttpRequestData req,
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

        [Function("ListarGradoAcademicoById")]
        public async Task<HttpResponseData> ListarGradoAcademicoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarGradoAcademicoById/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var gradoAcademico = await repository.Get(id);
                response = req.CreateResponse(gradoAcademico != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                if (gradoAcademico != null)
                {
                    await response.WriteAsJsonAsync(gradoAcademico);
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
