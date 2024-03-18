﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Direccion
    {
        [Key]
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }

        [ForeignKey("Persona")]
        public int IdPersona { get; set; }
        public Persona? Persona { get; set; }
    }
}
