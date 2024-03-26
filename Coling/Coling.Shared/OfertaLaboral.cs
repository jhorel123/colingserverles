using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coling.Shared
{
    public class OfertaLaboral
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int IdInstitucion { get; set; }
        public DateTime FechaOferta { get; set; }
        public DateTime FechaLimite { get; set; }
        public string? Descripcion { get; set; }
        public string? TituloCargo { get; set; }
        public string? TipoContrato { get; set; }
        public string? TipoTrabajo { get; set; }
        public string? Area { get; set; }
        public string? Caracteristicas { get; set; }
        public string? Estado { get; set; }
    }
}
