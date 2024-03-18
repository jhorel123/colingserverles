using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class PersonaTipoSocial
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Persona")]
        public int IdPersona { get; set; }

        public Persona? Persona { get; set; }

        [ForeignKey("TipoSocial")]
        public int IdTipoSocial { get; set; }

        public TipoSocial? TipoSocial { get; set; }

        public string? Estado { get; set; }
    }
}
