using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Implementacion
{
    public class OfertaLaboralLogic : IOfertaLaboralLogic
    {
        private readonly IMongoCollection<OfertaLaboral> _ofertasLaborales;

        public OfertaLaboralLogic(IConfiguration configuration)
        {
            var contexto = new Contexto(configuration);
            var connectionString = contexto.GetConnectionString();
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ColingBD");
            _ofertasLaborales = database.GetCollection<OfertaLaboral>("OfertaLaboral");
        }

        public async Task<bool> EliminarOfertaLaboral(OfertaLaboral ofertaLaboral)
        {
            try
            {
                var result = await _ofertasLaborales.DeleteOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", ofertaLaboral.Id));
                return result.DeletedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InsertarOfertaLaboral(OfertaLaboral ofertaLaboral)
        {
            try
            {
                await _ofertasLaborales.InsertOneAsync(ofertaLaboral);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<OfertaLaboral>> ListarOfertasLaborales()
        {
            try
            {
                return await _ofertasLaborales.Find(_ => true).ToListAsync();
            }
            catch (Exception)
            {
                return new List<OfertaLaboral>();
            }
        }

        public async Task<bool> ModificarOfertaLaboral(OfertaLaboral ofertaLaboral)
        {
            try
            {
                var result = await _ofertasLaborales.ReplaceOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", ofertaLaboral.Id), ofertaLaboral);
                return result.ModifiedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarOfertaLaboral(OfertaLaboral ofertaLaboral, string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var result = await _ofertasLaborales.ReplaceOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", objectId), ofertaLaboral);
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

        public async Task<OfertaLaboral> ObtenerOfertaLaboralById(string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    return await _ofertasLaborales.Find(Builders<OfertaLaboral>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
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

        public async Task<bool> EliminarOfertaLaboral(string id)
        {
            try
            {
                ObjectId objectId;
                if (ObjectId.TryParse(id, out objectId))
                {
                    var result = await _ofertasLaborales.DeleteOneAsync(Builders<OfertaLaboral>.Filter.Eq("_id", objectId));
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
