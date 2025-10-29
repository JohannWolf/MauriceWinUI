using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maurice.Data.Models
{
    public class UsoCFDI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(4)]
        public string Clave { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
