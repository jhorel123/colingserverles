using Coling.API.Curriculum.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface ITipoEstudioRepositorio
    {
        Task<bool> Create(TipoEstudio tipoEstudio);
        Task<bool> Update(TipoEstudio tipoEstudio);
        Task<bool> Delete(string partitionKey, string rowKey);
        Task<List<TipoEstudio>> GetAll();
        Task<TipoEstudio> Get(string id);
    }
}
