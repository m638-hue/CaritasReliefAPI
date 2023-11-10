using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;

namespace CaritasReliefAPI.Extensions
{
    [ExtendObjectType(typeof(Recolectores))]
    public class RecolectorExtensions
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recibos> GetRecibosActivos(
            [Parent] Recolectores recolector,
            SQLContext sqlContext,
            string date)
        {
            return sqlContext.Recibos
                .Where(r =>
                    r.idRecolector == recolector.idRecolector &&
                    r.fecha.Date.ToString() == date);
        }
    }
}
