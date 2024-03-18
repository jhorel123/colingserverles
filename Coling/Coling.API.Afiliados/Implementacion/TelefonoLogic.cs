using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    internal class TelefonoLogic : ITelefonoLogic
    {
        private readonly Contexto contexto;

        public TelefonoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarTelefono(Telefono telefono)
        {
            try
            {
                contexto.Telefonos.Remove(telefono);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarTelefono(int id)
        {
            try
            {
                var telefono = await contexto.Telefonos.FindAsync(id);

                if (telefono != null)
                {
                    contexto.Telefonos.Remove(telefono);
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

        public async Task<bool> InsertarTelefono(Telefono telefono)
        {
            try
            {
                contexto.Telefonos.Add(telefono);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Telefono>> ListarTelefonosPorPersona(int idPersona)
        {
            try
            {
                var telefonos = await contexto.Telefonos.Where(t => t.IdPersona == idPersona).ToListAsync();
                return telefonos;
            }
            catch (Exception)
            {
                return new List<Telefono>();
            }
        }

        public async Task<bool> ModificarTelefono(Telefono telefono)
        {
            try
            {
                contexto.Telefonos.Update(telefono);
                int response = await contexto.SaveChangesAsync();
                return response == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarTelefono(Telefono telefono, int id)
        {
            try
            {
                var telefonoExistente = await contexto.Telefonos.FindAsync(id);

                if (telefonoExistente != null)
                {
                    telefonoExistente.NroTelefono = telefono.NroTelefono;
                    telefonoExistente.Estado = telefono.Estado;

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

        public async Task<Telefono> ObtenerTelefonoById(int id)
        {
            try
            {
                var telefono = await contexto.Telefonos.FindAsync(id);
                return telefono;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
