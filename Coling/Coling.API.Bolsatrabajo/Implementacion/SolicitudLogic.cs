
using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Implementacion
{
    public class SolicitudLogic : ISolicitudLogic
    {
        private readonly IMongoCollection<Solicitud> _solicitudes;

        public SolicitudLogic(IConfiguration configuration)
        {
            var contexto = new Contexto(configuration);
            var connectionString = contexto.GetConnectionString();
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ColingBD");
            _solicitudes = database.GetCollection<Solicitud>("Solicitud");
        }

        public async Task<bool> EliminarSolicitud(Solicitud solicitud)
        {
            try
            {
                var result = await _solicitudes.DeleteOneAsync(Builders<Solicitud>.Filter.Eq("_id", solicitud.Id));
                return result.DeletedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InsertarSolicitud(Solicitud solicitud)
        {
            try
            {
                await _solicitudes.InsertOneAsync(solicitud);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Solicitud>> ListarSolicitudes()
        {
            try
            {
                return await _solicitudes.Find(_ => true).ToListAsync();
            }
            catch (Exception)
            {
                return new List<Solicitud>();
            }
        }

        public async Task<bool> ModificarSolicitud(Solicitud solicitud)
        {
            try
            {
                var result = await _solicitudes.ReplaceOneAsync(Builders<Solicitud>.Filter.Eq("_id", solicitud.Id), solicitud);
                return result.ModifiedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarSolicitud(Solicitud solicitud, string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var result = await _solicitudes.ReplaceOneAsync(Builders<Solicitud>.Filter.Eq("_id", objectId), solicitud);
                    return result.ModifiedCount == 1;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Solicitud> ObtenerSolicitudById(string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    return await _solicitudes.Find(Builders<Solicitud>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> EliminarSolicitud(string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var result = await _solicitudes.DeleteOneAsync(Builders<Solicitud>.Filter.Eq("_id", objectId));
                    return result.DeletedCount == 1;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
