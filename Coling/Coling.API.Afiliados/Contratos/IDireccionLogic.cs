using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IDireccionLogic
    {
        Task<bool> InsertarDireccion(Direccion direccion);
        Task<bool> ModificarDireccion(Direccion direccion, int id);
        Task<bool> EliminarDireccion(int id);
        Task<List<Direccion>> ListarDireccionesPorPersona(int idPersona);
        Task<Direccion> ObtenerDireccionById(int id);
    }
}
