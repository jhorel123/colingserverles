using Coling.Shared;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IPersonaLogic
    {
        public Task<bool> InsertarPersona(Persona persona);
        public Task<bool> ModificarPersona(Persona persona, int id);
        public Task<bool> EliminarPersona(int id);
        public Task<List<Persona>> ListarPersonaTodos();
        public Task<Persona> ObtenerPersonaById(int id);

    }
}
