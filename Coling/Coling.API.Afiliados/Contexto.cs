using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
namespace Coling.API.Afiliados
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        public virtual DbSet<Afiliado> Afiliados { get; set; }
        public virtual DbSet<Idioma> Idiomas { get; set; }
        public virtual DbSet<AfiliadoIdioma> AfiliadosIdiomas { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Direccion> Direcciones { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }
        public virtual DbSet<TipoSocial> TiposSociales { get; set; }
        public virtual DbSet<PersonaTipoSocial> PersonasTiposSociales { get; set; }
    }
}
