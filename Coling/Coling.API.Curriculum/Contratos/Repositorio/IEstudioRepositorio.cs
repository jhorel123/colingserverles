using Coling.API.Curriculum.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface IEstudioRepositorio
    {
        Task<bool> Create(Estudio estudio);
        Task<bool> Update(Estudio estudio);
        Task<bool> Delete(string partitionKey, string rowKey);
        Task<List<Estudio>> GetAll();
        Task<Estudio> Get(string id);
    }
}
