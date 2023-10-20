using System.ComponentModel.DataAnnotations;

namespace CaritasReliefAPI.Schema
{
    public class Logins
    {
        [Key]
        [GraphQLType("ID!")]
        [GraphQLName("id")]
        public int idLogin { get; set; }

        public string usuario { get; set; }

        public string contrasena { get; set; } 

        public int tipo { get; set; }

        public bool activo { get; set; }
    }
}
