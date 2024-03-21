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
    public class ExperienciaLaboralFunction
    {
        private readonly ILogger<ExperienciaLaboralFunction> _logger;
        private readonly IExperienciaLaboralRepositorio repositorio;

        public ExperienciaLaboralFunction(ILogger<ExperienciaLaboralFunction> logger, IExperienciaLaboralRepositorio repositorio)
        {
            _logger = logger;
            this.repositorio = repositorio;
        }

        [Function("InsertarExperienciaLaboral")]
        public async Task<HttpResponseData> InsertarExperienciaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {

                var experiencia = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar una experiencia laboral con todos sus datos");
                experiencia.RowKey = Guid.NewGuid().ToString();
                experiencia.Timestamp = DateTimeOffset.UtcNow;

                bool success = await repositorio.Create(experiencia);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("ListarExperienciasLaborales")]
        public async Task<HttpResponseData> ListarExperienciasLaborales(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                var listaExperiencias = await repositorio.GetAll();
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(listaExperiencias);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("EditarExperienciaLaboral")]
        public async Task<HttpResponseData> EditarExperienciaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarExperienciaLaboral/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var experiencia = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar una experiencia laboral con todos sus datos");
                experiencia.RowKey = id;
                bool success = await repositorio.Update(experiencia);
                response = req.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [Function("BorrarExperienciaLaboral")]
        public async Task<HttpResponseData> BorrarExperienciaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "BorrarExperienciaLaboral/{partitionKey}/{rowKey}")] HttpRequestData req,
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

        [Function("ListarExperienciaLaboralById")]
        public async Task<HttpResponseData> ListarExperienciaLaboralById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarExperienciaLaboralById/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData response;
            try
            {
                var experiencia = await repositorio.Get(id);
                response = req.CreateResponse(experiencia != null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
                if (experiencia != null)
                {
                    await response.WriteAsJsonAsync(experiencia);
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
