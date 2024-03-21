using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class EstudioRepositorio : IEstudioRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public EstudioRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Estudio";
        }

        public async Task<bool> Create(Estudio estudio)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(estudio);
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
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionKey, rowKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Estudio> Get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Estudios' and RowKey eq '{id}'";
            await foreach (Estudio estudio in tablaCliente.QueryAsync<Estudio>(filter: filtro))
            {
                return estudio;
            }
            return null;
        }

        public async Task<List<Estudio>> GetAll()
        {
            List<Estudio> lista = new List<Estudio>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Estudios'";

            await foreach (Estudio estudio in tablaCliente.QueryAsync<Estudio>(filter: filtro))
            {
                lista.Add(estudio);
            }

            return lista;
        }

        public async Task<bool> Update(Estudio estudio)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(estudio, estudio.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
