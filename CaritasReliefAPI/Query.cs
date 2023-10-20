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
        public IQueryable<Donantes> GetDonantes(SQLContext sqlContext, int? id = null)
        {
            if (id == null)
                return sqlContext.Donantes.AsQueryable();

            return sqlContext.Donantes.Where(d => d.idDonante == id);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recolectores> GetRecolectores(SQLContext sqlContext, int? id = null)
        {
            if (id == null) 
                return sqlContext.Recolectores.AsQueryable();

            return sqlContext.Recolectores.Where(r => r.idRecolector == id);
        }

        public async Task<HttpStatusCode> CobrarRecibo(SQLContext sqlContext, int id)
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
