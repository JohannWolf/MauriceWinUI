using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maurice.Data.Models
{
    public abstract class Comprobante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Common fields for all document types
        [Required]
        public string TipoDeDocumento { get; set; }
        [Required]
        public string Folio { get; set; }
        [Required]
        public DateTime? Fecha { get; set; }
        [Required]
        public string UUID { get; set; }

        // Emisor
        [Required]
        [MaxLength(13)]
        public string RfcEmisor { get; set; }
        public string NombreEmisor { get; set; }

        // Receptor
        [Required]
        [MaxLength(13)]
        public string RfcReceptor { get; set; }

        // Totales (common to both)
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        // Metadata
        public string FileName { get; set; }
        public DateTime ProcessedDate { get; set; } = DateTime.Now;

        // Common methods
        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(UUID) && !string.IsNullOrEmpty(RfcEmisor);
        }
    }
}
