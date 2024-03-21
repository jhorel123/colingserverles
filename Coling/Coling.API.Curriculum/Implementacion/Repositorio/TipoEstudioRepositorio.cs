using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class TipoEstudioRepositorio : ITipoEstudioRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public TipoEstudioRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "TipoEstudio";
        }

        public async Task<bool> Create(TipoEstudio tipoEstudio)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpsertEntityAsync(tipoEstudio);
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

        public async Task<TipoEstudio> Get(string id)
        {
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'TipoEstudios' and RowKey eq '{id}'";
            await foreach (TipoEstudio tipoEstudio in tableClient.QueryAsync<TipoEstudio>(filter: filter))
            {
                return tipoEstudio;
            }
            return null;
        }

        public async Task<List<TipoEstudio>> GetAll()
        {
            List<TipoEstudio> lista = new List<TipoEstudio>();
            var tableClient = new TableClient(cadenaConexion, tablaNombre);
            var filter = $"PartitionKey eq 'TipoEstudios'";

            await foreach (TipoEstudio tipoEstudio in tableClient.QueryAsync<TipoEstudio>(filter: filter))
            {
                lista.Add(tipoEstudio);
            }

            return lista;
        }

        public async Task<bool> Update(TipoEstudio tipoEstudio)
        {
            try
            {
                var tableClient = new TableClient(cadenaConexion, tablaNombre);
                await tableClient.UpdateEntityAsync(tipoEstudio, tipoEstudio.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
