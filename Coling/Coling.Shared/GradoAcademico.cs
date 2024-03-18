using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class GradoAcademico
    {
        [Key]
        public int id { get; set; }
        public string? NombreGrado { get; set; }
        public string? Estado { get; set; }
    }
}
