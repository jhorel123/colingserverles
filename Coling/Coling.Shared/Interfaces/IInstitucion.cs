using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared.Interfaces
{
    public interface IInstitucion
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
    }
}
