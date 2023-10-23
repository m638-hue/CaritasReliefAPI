using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;

namespace CaritasReliefAPI.Extensions
{
    [ExtendObjectType(typeof(Donantes))]
    public class DonanteExtension
    {
        public int GetCantidadRecibosActivos(
            [Parent] Donantes donante,
            SQLContext sqlContext,
            string date, 
            int idRecolector) 
        {
            return sqlContext.Recibos
                .Where(r => 
                    r.idDonante == donante.idDonante &&
                    r.idRecolector == idRecolector &&
                    r.fecha.Date.ToString() == date &&
                    !r.cobrado)
                .Count();
        }

        public IQueryable<Recibos> GetRecibosActivos(
            [Parent] Donantes donante,
            SQLContext sqlContext,
            string date,
            int idRecolector)
        {
            return sqlContext.Recibos
                .Where(r =>
                    r.idDonante == donante.idDonante &&
                    r.idRecolector == idRecolector &&
                    r.fecha.Date.ToString() == date &&
                   !r.cobrado);
        }
    }
}
