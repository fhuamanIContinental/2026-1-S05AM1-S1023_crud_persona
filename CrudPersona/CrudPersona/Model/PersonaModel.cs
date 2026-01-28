using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudPersona.Model
{

    [Table("persona")]
    public class PersonaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(20)]
        public string? TipoDocumento { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        public string? NumeroDocumento { get; set; }

        [StringLength(100)]
        public string? Nombres { get; set; }

        [StringLength(100)]
        public string? ApellidoPaterno { get; set; }

        [StringLength(100)]
        public string? ApellidoMaterno { get; set; }

        [StringLength(100)]
        public string? Direccion { get; set; }

        [StringLength(20)]
        public string? Celular { get; set; }
    }

}
