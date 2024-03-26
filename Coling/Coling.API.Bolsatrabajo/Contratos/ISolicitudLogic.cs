using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos
{
    public interface ISolicitudLogic

    {
        Task<bool> InsertarSolicitud(Solicitud solicitud);
        Task<bool> ModificarSolicitud(Solicitud solicitud, string id);
        Task<bool> EliminarSolicitud(string id);
        Task<List<Solicitud>> ListarSolicitudes();
        Task<Solicitud> ObtenerSolicitudById(string id);
    }
}
