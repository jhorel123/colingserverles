using Coling.API.Curriculum.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface IExperienciaLaboralRepositorio
    {
        Task<bool> Create(ExperienciaLaboral experienciaLaboral);
        Task<bool> Update(ExperienciaLaboral experienciaLaboral);
        Task<bool> Delete(string partitionKey, string rowKey);
        Task<List<ExperienciaLaboral>> GetAll();
        Task<ExperienciaLaboral> Get(string id);
    }
}
