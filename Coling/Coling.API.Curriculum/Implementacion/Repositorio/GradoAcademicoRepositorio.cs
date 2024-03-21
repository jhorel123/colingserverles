using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class GradoAcademicoRepositorio : IGradoAcademicoRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public GradoAcademicoRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "GradoAcademico";
        }

        public async Task<bool> Create(GradoAcademico gradoAcademico)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpsertEntityAsync(gradoAcademico);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string partitionKey, string rowKey)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.DeleteEntityAsync(partitionKey, rowKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<GradoAcademico> Get(string id)
        {
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'GradosAcademicos' and RowKey eq '{id}'";
            await foreach (GradoAcademico gradoAcademico in tableClient.QueryAsync<GradoAcademico>(filter: filter))
            {
                return gradoAcademico;
            }
            return null;
        }

        public async Task<List<GradoAcademico>> GetAll()
        {
            List<GradoAcademico> lista = new List<GradoAcademico>();
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'GradosAcademicos'";

            await foreach (GradoAcademico gradoAcademico in tableClient.QueryAsync<GradoAcademico>(filter: filter))
            {
                lista.Add(gradoAcademico);
            }

            return lista;
        }

        public async Task<bool> Update(GradoAcademico gradoAcademico)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpdateEntityAsync(gradoAcademico, gradoAcademico.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
