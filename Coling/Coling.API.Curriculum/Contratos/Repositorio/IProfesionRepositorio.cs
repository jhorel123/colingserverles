using Coling.API.Curriculum.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface IProfesionRepositorio
    {
        Task<bool> Create(Profesion profesion);
        Task<bool> Update(Profesion profesion);
        Task<bool> Delete(string partitionKey, string rowKey);
        Task<List<Profesion>> GetAll();
        Task<Profesion> Get(string id);
    }
}
