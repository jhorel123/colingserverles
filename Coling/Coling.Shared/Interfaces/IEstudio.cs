using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared.Interfaces
{
    public interface IEstudio
    {
        public string TipoEstudio { get; set; }
        public string Afiliado { get; set; }
        public string Grado { get; set; }
        public string TituloRecibido { get; set; }
        public string Institucion { get; set; }
        public int Año { get; set; }
        public string Estado { get; set; }
    }
}
