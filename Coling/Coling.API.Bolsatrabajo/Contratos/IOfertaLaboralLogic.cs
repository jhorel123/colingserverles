using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos
{
    public interface IOfertaLaboralLogic
    {
        Task<bool> InsertarOfertaLaboral(OfertaLaboral ofertaLaboral);
        Task<bool> ModificarOfertaLaboral(OfertaLaboral ofertaLaboral, string id);
        Task<bool> EliminarOfertaLaboral(string id);
        Task<List<OfertaLaboral>> ListarOfertasLaborales();
        Task<OfertaLaboral> ObtenerOfertaLaboralById(string id);
    }
}
