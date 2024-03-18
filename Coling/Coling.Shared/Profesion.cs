using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Profesion
    {
        [Key]
        public int Id { get; set; }
        public string? NombreProfesion { get; set; }
        public string? Estado { get; set; }

        [ForeignKey("GradpAcademico")]
        public int IdGradpAcademico { get; set; }
        public GradoAcademico? GradoAcademico { get; set; }
    }
}
