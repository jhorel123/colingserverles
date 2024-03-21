using Coling.API.Curriculum.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface IGradoAcademicoRepositorio
    {
        Task<bool> Create(GradoAcademico gradoAcademico);
        Task<bool> Update(GradoAcademico gradoAcademico);
        Task<bool> Delete(string partitionKey, string rowKey);
        Task<List<GradoAcademico>> GetAll();
        Task<GradoAcademico> Get(string id);
    }
}
