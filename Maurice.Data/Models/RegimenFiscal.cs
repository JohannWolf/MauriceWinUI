using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maurice.Data.Models
{
    public class RegimenFiscal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public int Clave { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
