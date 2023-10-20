using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaritasReliefAPI.Schema
{
    public class Recibos
    {
        [Key]
        [GraphQLType("ID!")]
        [GraphQLName("id")]
        public int idRecibo { get; set; }

        public int idRecolector { get; set; }

        public int idDonante { get; set; } 

        public decimal cantidad { get; set; }  

        public bool cobrado { get; set; }  

        public string comentarios { get; set; }

        public DateTime fecha { get; set; }

        public string comentarioHorario { get; set; }

        public bool activo { get; set; }

        [ForeignKey("idRecolector")]
        public Recolectores recolector { get; set; }

        [ForeignKey("idDonante")]
        public Donantes donante { get; set; }
    }
}
