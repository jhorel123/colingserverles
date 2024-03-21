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
    public class ExperienciaLaboral : IExperienciaLaboral, ITableEntity
    {
        public string Afiliado { get; set; }
        public string Institucion { get; set; }
        public string CargoTitulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Estado { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
