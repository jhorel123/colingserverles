using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface ITelefonoLogic
    {
        Task<bool> InsertarTelefono(Telefono telefono);
        Task<bool> ModificarTelefono(Telefono telefono, int id);
        Task<bool> EliminarTelefono(int id);
        Task<List<Telefono>> ListarTelefonosPorPersona(int idPersona);
        Task<Telefono> ObtenerTelefonoById(int id);
    }
}
