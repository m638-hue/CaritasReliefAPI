using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaritasReliefAPI.Schema
{
    public class Recolectores
    {
        [Key]
        [GraphQLType("ID!")]
        [GraphQLName("id")]
        public int idRecolector { get; set; }

        public int idLogin { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public bool activo { get; set; }


        // Relationship
        [ForeignKey("idLogin")]
        public Logins login { get; set; }

        public IEnumerable<Recibos> recibos { get; set; }
    }
}
