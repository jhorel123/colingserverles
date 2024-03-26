using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coling.Shared
{
    public class Solicitud
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int IdAfiliado { get; set; }
        public int? IdOferta { get; set; }
        public string? Nombre { get; set; }
        public string? FechaPostulacion { get; set; }
        public int? PretencionSalarial { get; set; }
        public string? Acercade { get; set; }
        public string? Estado { get; set; }
    }
}
