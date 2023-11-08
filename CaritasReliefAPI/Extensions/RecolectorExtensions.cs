using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;

namespace CaritasReliefAPI.Extensions
{
    [ExtendObjectType(typeof(Recolectores))]
    public class RecolectorExtensions
    {
        public IQueryable<Recibos> GetRecibosActivos(
            [Parent] Recolectores recolector,
            SQLContext sqlContext,
            string date,
            int idRecolector)
        {
            return sqlContext.Recibos
                .Where(r =>
                    r.idDonante == recolector.idRecolector &&
                    r.idRecolector == idRecolector &&
                    r.fecha.Date.ToString() == date);
        }
    }
}
