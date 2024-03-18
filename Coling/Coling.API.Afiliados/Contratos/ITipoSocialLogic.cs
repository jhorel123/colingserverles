using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface ITipoSocialLogic
    {
        Task<bool> InsertarTipoSocial(TipoSocial tipoSocial);
        Task<bool> ModificarTipoSocial(TipoSocial tipoSocial, int id);
        Task<bool> EliminarTipoSocial(int id);
        Task<List<TipoSocial>> ListarTiposSocial();
        Task<TipoSocial> ObtenerTipoSocialById(int id);
    }
}
