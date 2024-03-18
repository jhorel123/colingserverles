using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Coling.API.Afiliados.Contratos
{
    public interface IAfiliadoIdiomaLogic
    {
        Task<bool> InsertarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma);
        Task<bool> ModificarAfiliadoIdioma(AfiliadoIdioma afiliadoIdioma, int id);
        Task<bool> EliminarAfiliadoIdioma(int id);
        Task<List<AfiliadoIdioma>> ListarAfiliadoIdiomas();
        Task<AfiliadoIdioma> ObtenerAfiliadoIdiomaById(int id);
    }
}
