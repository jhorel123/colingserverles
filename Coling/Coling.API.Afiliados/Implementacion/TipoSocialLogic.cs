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
    public class TipoSocialLogic : ITipoSocialLogic
    {
        private readonly Contexto contexto;

        public TipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarTipoSocial(TipoSocial tipoSocial)
        {
            try
            {
                contexto.TiposSociales.Remove(tipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarTipoSocial(int id)
        {
            try
            {
                var tipoSocial = await contexto.TiposSociales.FindAsync(id);

                if (tipoSocial != null)
                {
                    contexto.TiposSociales.Remove(tipoSocial);
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

        public async Task<bool> InsertarTipoSocial(TipoSocial tipoSocial)
        {
            try
            {
                contexto.TiposSociales.Add(tipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<TipoSocial>> ListarTiposSocial()
        {
            try
            {
                var lista = await contexto.TiposSociales.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<TipoSocial>();
            }
        }

        public async Task<bool> ModificarTipoSocial(TipoSocial tipoSocial)
        {
            try
            {
                contexto.TiposSociales.Update(tipoSocial);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarTipoSocial(TipoSocial tipoSocial, int id)
        {
            try
            {
                var tipoSocialExistente = await contexto.TiposSociales.FindAsync(id);

                if (tipoSocialExistente != null)
                {
                    tipoSocialExistente.NombreSocial = tipoSocial.NombreSocial;
                    tipoSocialExistente.Estado = tipoSocial.Estado;

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

        public async Task<TipoSocial> ObtenerTipoSocialById(int id)
        {
            try
            {
                var tipoSocial = await contexto.TiposSociales.FindAsync(id);
                return tipoSocial;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
