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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarExperienciaLaboral", "InsertarExperienciaLaboral", Description = "Sirve para ingresar una experiencia laboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "Ingresar una nueva experiencia laboral")]
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarExperienciasLaborales", "ListarExperienciasLaborales", Description = "Sirve para listar todas las experiencias laborales")]
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EditarExperienciaLaboral", "EditarExperienciaLaboral", Description = "Sirve para editar una experiencia laboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "Editar experiencia laboral")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la experiencia laboral", Description = "El ID de la experiencia laboral a editar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EditarExperienciaLaboral(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarExperienciaLaboral/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var experiencia = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar una experiencia laboral con todos sus datos");
                experiencia.RowKey = rowKey;
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("BorrarExperienciaLaboral", "BorrarExperienciaLaboral", Description = "Sirve para eliminar una experiencia laboral")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la experiencia laboral", Description = "El PartitionKey de la experiencia laboral a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la experiencia laboral", Description = "El RowKey de la experiencia laboral a borrar", Visibility = OpenApiVisibilityType.Important)]
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarExperienciaLaboralById", "ListarExperienciaLaboralById", Description = "Sirve para obtener una experiencia laboral por su ID")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la experiencia laboral", Description = "El ID de la experiencia laboral a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarExperienciaLaboralById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarExperienciaLaboralById/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var experiencia = await repositorio.Get(rowKey);
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
