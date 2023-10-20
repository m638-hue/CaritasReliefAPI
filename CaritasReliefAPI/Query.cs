using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;
using HotChocolate.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace CaritasReliefAPI
{
    [Authorize]
    public class Query
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
        public IQueryable<Donantes> GetDonantes(SQLContext sqlContext)
            => sqlContext.Donantes.AsQueryable();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recolectores> GetRecolectores(SQLContext sqlContext)
            => sqlContext.Recolectores.AsQueryable();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recolectores> GetRecolectoresById(SQLContext sqlContext, int id)
        {
            return sqlContext.Recolectores.Where(r => r.idRecolector == id).AsQueryable();
        }

        public async Task<HttpStatusCode> CobrarRecibo (SQLContext sqlContext, int id)
        {
            var recibo = await sqlContext.Recibos.FindAsync(id);

            if (recibo == null)
                return HttpStatusCode.InternalServerError;

            recibo.cobrado = true;
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
                .AsQueryable();
        }
    }
}
