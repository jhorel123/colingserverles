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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("InsertarProfesion", "InsertarProfesion", Description = "Sirve para ingresar una profesión")]
        [OpenApiRequestBody("application/json", typeof(Profesion), Description = "Ingresar profesión nueva")]
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarProfesiones", "ListarProfesiones", Description = "Sirve para listar todas las profesiones")]
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("EditarProfesion", "EditarProfesion", Description = "Sirve para editar una profesión")]
        [OpenApiRequestBody("application/json", typeof(Profesion), Description = "Editar profesión")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la profesión", Description = "El RowKey de la profesión a editar", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EditarProfesion(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "EditarProfesion/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una profesión con todos sus datos");
                profesion.RowKey = rowKey;
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
        [ColingAuthorize(AplicacionRoles.Admin)]
        [OpenApiOperation("BorrarProfesion", "BorrarProfesion", Description = "Sirve para eliminar una profesión")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la profesión", Description = "El PartitionKey de la profesión a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la profesión", Description = "El RowKey de la profesión a borrar", Visibility = OpenApiVisibilityType.Important)]
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
        [ColingAuthorize(AplicacionRoles.Admin + "," + AplicacionRoles.Secretaria + "," + AplicacionRoles.Afiliado)]
        [OpenApiOperation("ListarProfesionById", "ListarProfesionById", Description = "Sirve para obtener una profesión por su ID")]
        [OpenApiParameter(name: "rowKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la profesión", Description = "El RowKey de la profesión a obtener", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> ListarProfesionById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarProfesionById/{rowKey}")] HttpRequestData req,
            string rowKey)
        {
            HttpResponseData response;
            try
            {
                var profesion = await repository.Get(rowKey);
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
