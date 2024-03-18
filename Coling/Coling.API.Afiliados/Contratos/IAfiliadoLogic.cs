using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Coling.API.Afiliados.Contratos
{
    public interface IAfiliadoLogic
    {
        Task<bool> InsertarAfiliado(Afiliado afiliado);
        Task<bool> ModificarAfiliado(Afiliado afiliado, int id);
        Task<bool> EliminarAfiliado(int id);
        Task<List<Afiliado>> ListarAfiliados();
        Task<Afiliado> ObtenerAfiliadoById(int id);
    }
}
