using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaMvcCore2APJ.Models
{
    [Table("GENEROS")]
    public class Genero
    {
        [Key]
        [Column("IDGENERO")]
        public int IdGenero { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
    }
}
