using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class IdiomaLogic : IIdiomaLogic
    {
        private readonly Contexto contexto;

        public IdiomaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarIdioma(Idioma idioma)
        {
            try
            {
                contexto.Idiomas.Remove(idioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarIdioma(int id)
        {
            try
            {
                var idioma = await contexto.Idiomas.FindAsync(id);

                if (idioma != null)
                {
                    contexto.Idiomas.Remove(idioma);
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

        public async Task<bool> InsertarIdioma(Idioma idioma)
        {
            try
            {
                contexto.Idiomas.Add(idioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Idioma>> ListarIdiomas()
        {
            try
            {
                var lista = await contexto.Idiomas.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<Idioma>();
            }
        }

        public async Task<bool> ModificarIdioma(Idioma idioma)
        {
            try
            {
                contexto.Idiomas.Update(idioma);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarIdioma(Idioma idioma, int id)
        {
            try
            {
                var idiomaExistente = await contexto.Idiomas.FindAsync(id);

                if (idiomaExistente != null)
                {
                    idiomaExistente.NombreIdioma = idioma.NombreIdioma;
                    idiomaExistente.Estado = idioma.Estado;

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

        public async Task<Idioma> ObtenerIdiomaById(int id)
        {
            try
            {
                var idioma = await contexto.Idiomas.FindAsync(id);
                return idioma;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
