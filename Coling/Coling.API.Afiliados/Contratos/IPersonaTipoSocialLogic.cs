using Coling.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IPersonaTipoSocialLogic
    {
        Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial);
        Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial, int id);
        Task<bool> EliminarPersonaTipoSocial(int id);
        Task<List<PersonaTipoSocial>> ListarPersonaTiposSocial();
        Task<PersonaTipoSocial> ObtenerPersonaTipoSocialById(int id);
    }
}
