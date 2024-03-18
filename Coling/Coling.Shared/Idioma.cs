using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Idioma
    {
        [Key]
        public int Id { get; set; }

        public string? NombreIdioma { get; set; }

        public string? Estado { get; set; }

        public List<AfiliadoIdioma> AfiliadoIdioma { get; set; }
    }
}
