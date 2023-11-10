using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Net;

namespace CaritasReliefAPI
{
    public partial class Query
    {
        [Authorize(Roles = new string[] {"admin"})]
        [UseProjection]
        public EstadoRecibosDelDia GetEstadoRecibos (SQLContext context, string date, int? idRecolector = null)
        {
            var recibos = 
                idRecolector == null ? 
                context.Recibos : 
                context.Recibos.Where(r => r.idRecolector == idRecolector);

            return new EstadoRecibosDelDia
            {
                cobradosFallidos = context.Recibos.Count(r => r.cobrado == 0 && r.fecha.Date.ToString() == date),
                cobrados = recibos.Count(r => r.cobrado == 1 && r.fecha.Date.ToString() == date),
                pendiente = recibos.Count(r => r.cobrado == 2 && r.fecha.Date.ToString() == date)
            };
        }

        [Authorize(Roles = new string[] { "admin" })]
        public decimal GetTotalCobrado (SQLContext context, string date, int? idRecolector = null)
        {
            var recibos =
                idRecolector == null ?
                context.Recibos :
                context.Recibos.Where(r => r.idRecolector == idRecolector);

            return recibos
                .Where(r => r.cobrado == 1 && r.fecha.Date.ToString() == date)
                .Sum(r => r.cantidad);
        }

        [Authorize(Roles = new string[] {"admin"})]
        public async Task<HttpStatusCode> TransferirARecolector (SQLContext context, int idRecibo, int idRecolector)
        {
            var recibo = await context.Recibos.FindAsync(idRecibo);

            if (recibo == null) return HttpStatusCode.BadRequest;

            recibo.idRecolector = idRecolector;

            await context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}
