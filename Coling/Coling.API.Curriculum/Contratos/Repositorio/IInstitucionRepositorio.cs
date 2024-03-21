using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coling.API.Curriculum.Modelo;
using Coling.Shared;

namespace Coling.API.Curriculum.Contratos.Repositorio
{
    public interface IInstitucionRepositorio
    {
        public Task<bool> Create(Institucion institucion);
        public Task<bool> Update(Institucion institucion);
        public Task<bool> Delete(string partitionkey, string rowkey);
        public Task<List<Institucion>> GetAll();
        public Task<Institucion> Get(string id);
    }
}
