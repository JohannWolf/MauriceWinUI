using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Data.Models
{
    public abstract class Comprobante
    {
        // Common fields for all document types
        public string TipoDeDocumento { get; set; }
        public string Folio { get; set; }
        public DateTime? Fecha { get; set; }
        public string UUID { get; set; }

        // Emisor
        public string RfcEmisor { get; set; }
        public string NombreEmisor { get; set; }

        // Receptor
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
