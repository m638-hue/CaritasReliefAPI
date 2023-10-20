using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaritasReliefAPI.Schema
{
    public class Admin
    {
        [Key]
        [GraphQLType("ID!")]
        [GraphQLName("id")]
        public int idAdmin { get; set; }

        public int idLogin { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public bool activo { get; set; }

        [ForeignKey("idLogin")]
        public Logins login { get; set; }
    }
}
