using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class AfiliadoIdiomaLogic : IAfiliadoIdiomaLogic
    {
        private readonly Contexto contexto;

        public AfiliadoIdiomaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma)
        {
            try
            {
                contexto.AfiliadosIdiomas.Remove(afiliadoIdioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarAfiliadoIdioma(int id)
        {
            try
            {
                var afiliadoIdioma = await contexto.AfiliadosIdiomas.FindAsync(id);

                if (afiliadoIdioma != null)
                {
                    contexto.AfiliadosIdiomas.Remove(afiliadoIdioma);
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

        public async Task<bool> InsertarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma)
        {
            try
            {
                contexto.AfiliadosIdiomas.Add(afiliadoIdioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<AfiliadoIdioma>> ListarAfiliadoIdiomas()
        {
            try
            {
                var lista = await contexto.AfiliadosIdiomas.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<AfiliadoIdioma>();
            }
        }

        public async Task<bool> ModificarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma)
        {
            try
            {
                contexto.AfiliadosIdiomas.Update(afiliadoIdioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma, int id)
        {
            try
            {
                var afiliadoIdiomaExistente = await contexto.AfiliadosIdiomas.FindAsync(id);

                if (afiliadoIdiomaExistente != null)
                {
                    afiliadoIdiomaExistente.IdAfiliado = afiliadoIdioma.IdAfiliado;
                    afiliadoIdiomaExistente.IdIdioma = afiliadoIdioma.IdIdioma;

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

        public async Task<AfiliadoIdioma> ObtenerAfiliadoIdiomaById(int id)
        {
            try
            {
                var afiliadoIdioma = await contexto.AfiliadosIdiomas.FindAsync(id);
                return afiliadoIdioma;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
