using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared.Interfaces
{
    public interface IExperienciaLaboral
    {
        public string Afiliado { get; set; }
        public string Institucion { get; set; }
        public string CargoTitulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Estado { get; set; }
    }
}
