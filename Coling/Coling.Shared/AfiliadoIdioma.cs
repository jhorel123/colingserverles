using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class AfiliadoIdioma
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Afiliado")]
        public int IdAfiliado { get; set; }

        public Afiliado? Afiliado { get; set; }

        [ForeignKey("Idioma")]
        public int IdIdioma { get; set; }

        public Idioma? Idioma { get; set; }

    }
}
