using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class PersonaTipoSocialLogic : IPersonaTipoSocialLogic
    {
        private readonly Contexto contexto;

        public PersonaTipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial)
        {
            try
            {
                contexto.PersonasTiposSociales.Remove(personaTipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarPersonaTipoSocial(int id)
        {
            try
            {
                var personaTipoSocial = await contexto.PersonasTiposSociales.FindAsync(id);

                if (personaTipoSocial != null)
                {
                    contexto.PersonasTiposSociales.Remove(personaTipoSocial);
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

        public async Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial)
        {
            try
            {
                contexto.PersonasTiposSociales.Add(personaTipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<PersonaTipoSocial>> ListarPersonaTiposSocial()
        {
            try
            {
                var lista = await contexto.PersonasTiposSociales.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<PersonaTipoSocial>();
            }
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial)
        {
            try
            {
                contexto.PersonasTiposSociales.Update(personaTipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial, int id)
        {
            try
            {
                var personaTipoSocialExistente = await contexto.PersonasTiposSociales.FindAsync(id);

                if (personaTipoSocialExistente != null)
                {
                    personaTipoSocialExistente.IdTipoSocial = personaTipoSocial.IdTipoSocial;
                    personaTipoSocialExistente.IdPersona = personaTipoSocial.IdPersona;
                    personaTipoSocialExistente.Estado = personaTipoSocial.Estado;

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

        public async Task<PersonaTipoSocial> ObtenerPersonaTipoSocialById(int id)
        {
            try
            {
                var personaTipoSocial = await contexto.PersonasTiposSociales.FindAsync(id);
                return personaTipoSocial;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
