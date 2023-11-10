using Microsoft.EntityFrameworkCore;

namespace CaritasReliefAPI.Schema
{
    [Keyless]
    public class EstadoRecibosDelDia
    {
        public int cobrados { get; set; }

        public int pendiente { get; set; }  

        public int cobradosFallidos { get; set; }
    }
}
