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
        public int TipoDeTransaccion { get; set; }
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
        //Total Payment after deductions in Nomina or plus taxes in Factura
        public decimal Total { get; set; }

        // Metadata
        public string FileName { get; set; }
        public DateTime ProcessedDate { get; set; } = DateTime.Now;

        // Common methods
        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(UUID) && !string.IsNullOrEmpty(RfcEmisor);
        }

        public abstract decimal GetIncomeAmount();
        public abstract decimal GetExpenseAmount();
        public abstract decimal GetIVAAmount();
        public abstract decimal GetISRAmount();
    }
}
