using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class TipoSocial
    {
        [Key]
        public int Id { get; set; }

        public string? NombreSocial { get; set; }

        public string? Estado { get; set; }

        public List<PersonaTipoSocial> PersonaTipoSocial { get; set; }
    }
}
