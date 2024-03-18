using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class AfiliadoLogic : IAfiliadoLogic
    {
        private readonly Contexto contexto;

        public AfiliadoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarAfiliado(Afiliado afiliado)
        {
            try
            {
                contexto.Afiliados.Remove(afiliado);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarAfiliado(int id)
        {
            try
            {
                var afiliado = await contexto.Afiliados.FindAsync(id);

                if (afiliado != null)
                {
                    contexto.Afiliados.Remove(afiliado);
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

        public async Task<bool> InsertarAfiliado(Afiliado afiliado)
        {
            try
            {
                contexto.Afiliados.Add(afiliado);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Afiliado>> ListarAfiliados()
        {
            try
            {
                var lista = await contexto.Afiliados.ToListAsync();
                return lista;
            }
            catch (Exception)
            {
                return new List<Afiliado>();
            }
        }

        public async Task<bool> ModificarAfiliado(Afiliado afiliado)
        {
            try
            {
                contexto.Afiliados.Update(afiliado);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarAfiliado(Afiliado afiliado, int id)
        {
            try
            {
                var afiliadoExistente = await contexto.Afiliados.FindAsync(id);

                if (afiliadoExistente != null)
                {
                    afiliadoExistente.FechaAfiliacion = afiliado.FechaAfiliacion;
                    afiliadoExistente.CodigoAfiliado = afiliado.CodigoAfiliado;
                    afiliadoExistente.NroTituloProvisional = afiliado.NroTituloProvisional;
                    afiliadoExistente.Estado = afiliado.Estado;
                    afiliadoExistente.IdPersona = afiliado.IdPersona;

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

        public async Task<Afiliado> ObtenerAfiliadoById(int id)
        {
            try
            {
                var afiliado = await contexto.Afiliados.FindAsync(id);
                return afiliado;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
