using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
        public string? Estado { get; set; }
        public List<Direccion> Direcciones { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public List<PersonaTipoSocial> PersonaTipoSocial { get; set; }
        public List<Afiliado> Afiliados { get; set; }
    }
}
