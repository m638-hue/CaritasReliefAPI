using System.ComponentModel.DataAnnotations;

namespace CaritasReliefAPI.Schema
{
    public class Donantes
    {
        [Key]
        [GraphQLType("ID!")]
        [GraphQLName("id")]
        public int idDonante { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public string direccion { get; set; }

        public string referenciaDomicilio { get; set; }

        public decimal latitude { get; set; }

        public decimal longitude { get; set; }

        public string telCasa { get; set; }

        public string telCelular { get; set; }

        public bool activo { get; set; }

        // Relacion
        public IEnumerable<Recibos> recibos { get; set; }
    }
}
