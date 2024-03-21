using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class ProfesionRepositorio : IProfesionRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ProfesionRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Profesion";
        }

        public async Task<bool> Create(Profesion profesion)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpsertEntityAsync(profesion);
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

        public async Task<Profesion> Get(string id)
        {
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'Profesiones' and RowKey eq '{id}'";
            await foreach (Profesion profesion in tableClient.QueryAsync<Profesion>(filter: filter))
            {
                return profesion;
            }
            return null;
        }

        public async Task<List<Profesion>> GetAll()
        {
            List<Profesion> lista = new List<Profesion>();
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'Profesiones'";

            await foreach (Profesion profesion in tableClient.QueryAsync<Profesion>(filter: filter))
            {
                lista.Add(profesion);
            }

            return lista;
        }

        public async Task<bool> Update(Profesion profesion)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpdateEntityAsync(profesion, profesion.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
