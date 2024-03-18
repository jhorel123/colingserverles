using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class PersonaLogic : IPersonaLogic
    {
        private readonly Contexto contexto;
        public PersonaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarPersona(Persona persona)
        {
            try
            {
                contexto.Personas.Remove(persona);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarPersona(int id)
        {
            try
            {
                var persona = await contexto.Personas.FindAsync(id);

                if (persona != null)
                {
                    contexto.Personas.Remove(persona);
                    int response = await contexto.SaveChangesAsync();
                    return response == 1;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InsertarPersona(Persona persona)
        {
            try
            {
                contexto.Personas.Add(persona);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Persona>> ListarPersonaTodos()
        {
            try
            {
                var lista = await contexto.Personas.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<Persona>();
            }
        }

        public async Task<bool> ModificarPersona(Persona persona)
        {
            try
            {
                contexto.Personas.Update(persona);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarPersona(Persona persona, int id)
        {
            try
            {
                var personaExistente = await contexto.Personas.FindAsync(id);

                if (personaExistente != null)
                {
                    personaExistente.Nombre = persona.Nombre;
                    personaExistente.Apellidos = persona.Apellidos;
                    personaExistente.FechaNacimiento = persona.FechaNacimiento;
                    personaExistente.Foto = persona.Foto;
                    personaExistente.Estado = persona.Estado;

                    int response = await contexto.SaveChangesAsync();
                    return response == 1;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Persona> ObtenerPersonaById(int id)
        {
            try
            {
                var persona = await contexto.Personas.FindAsync(id);
                return persona;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
