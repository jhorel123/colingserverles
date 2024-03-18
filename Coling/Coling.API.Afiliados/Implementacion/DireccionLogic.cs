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
    public class DireccionLogic : IDireccionLogic
    {
        private readonly Contexto contexto;
        public DireccionLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarDireccion(Direccion direccion)
        {
            try
            {
                contexto.Direcciones.Remove(direccion);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarDireccion(int id)
        {
            try
            {
                var direccion = await contexto.Direcciones.FindAsync(id);

                if (direccion != null)
                {
                    contexto.Direcciones.Remove(direccion);
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

        public async Task<bool> InsertarDireccion(Direccion direccion)
        {
            try
            {
                contexto.Direcciones.Add(direccion);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Direccion>> ListarDireccionesPorPersona(int idPersona)
        {
            try
            {
                var direcciones = await contexto.Direcciones.Where(d => d.IdPersona == idPersona).ToListAsync();
                return direcciones;
            }
            catch (Exception)
            {
                return new List<Direccion>();
            }
        }

        public async Task<bool> ModificarDireccion(Direccion direccion)
        {
            try
            {
                contexto.Direcciones.Update(direccion);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarDireccion(Direccion direccion, int id)
        {
            try
            {
                var direccionExistente = await contexto.Direcciones.FindAsync(id);

                if (direccionExistente != null)
                {
                    direccionExistente.Descripcion = direccion.Descripcion;
                    direccionExistente.Estado = direccion.Estado;

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

        public async Task<Direccion> ObtenerDireccionById(int id)
        {
            try
            {
                var direccion = await contexto.Direcciones.FindAsync(id);
                return direccion;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
