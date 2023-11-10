using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CaritasReliefAPI
{
    [Authorize]
    public partial class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recibos> GetRecibos(SQLContext sqlContext, int? id = null)
        {
            if (id == null)
                return sqlContext.Recibos.AsQueryable();

            return sqlContext.Recibos.AsQueryable().Where(x => x.idRecibo == id);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Donantes> GetDonantes(SQLContext sqlContext, int? id = null)
        {
            if (id == null)
                return sqlContext.Donantes.AsQueryable();

            return sqlContext.Donantes.Where(d => d.idDonante == id);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recolectores> GetRecolectores(SQLContext sqlContext)
        {
            return sqlContext.Recolectores.AsQueryable();
        }

        [UseProjection]
        public async Task<Recolectores?> GetRecolector(SQLContext context, int id)
        {
            return await context.Recolectores.FindAsync(id);
        }

        public async Task<HttpStatusCode> CobrarRecibo(SQLContext sqlContext, int id)
        {
            var recibo = await sqlContext.Recibos.FindAsync(id);

            if (recibo == null)
                return HttpStatusCode.InternalServerError;

            recibo.cobrado = 1;
            sqlContext.SaveChanges();

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> PostponerRecibo(SQLContext sqlContext, int id, string comentario)
        {
            var recibo = await sqlContext.Recibos.FindAsync(id);

            if (recibo == null)
                return HttpStatusCode.InternalServerError;

            recibo.comentarios = comentario;
            recibo.cobrado = 0;
            sqlContext.SaveChanges();

            return HttpStatusCode.OK;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Donantes> GetDonantesHoy(SQLContext sqlContext, string date, int idRecolector)
        {
            return sqlContext.Recibos
                .Where(r => r.fecha.Date.ToString() == date)
                .Where(r => r.idRecolector == idRecolector)
                .Select(r => r.donante)
                .Distinct();
        }

        public int GetCantidadRecibos(SQLContext sqlContext, string date, int idRecolector, int idDonante)
        {
            return sqlContext.Recibos
                .Where(r => 
                    r.fecha.Date.ToString() == date &&
                    r.idRecolector == idRecolector &&
                    r.idDonante == idDonante &&
                    r.cobrado == 0)
                .Count();
        }

    }
}
