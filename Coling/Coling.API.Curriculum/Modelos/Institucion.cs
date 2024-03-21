using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Coling.Shared.Interfaces;

namespace Coling.API.Curriculum.Modelo
{
    public class Institucion : IInstitucion, ITableEntity
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }
}
