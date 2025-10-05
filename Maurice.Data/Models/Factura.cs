using System.ComponentModel.DataAnnotations;

namespace Maurice.Data.Models
{
    public class Factura : Comprobante
    {
        // Factura-specific fields
        public string ClaveProdServ { get; set; }
        public string Descripcion { get; set; }

        // Impuestos
        [Required]
        public decimal Base { get; set; }
        public string Tasa { get; set; }
        public decimal ImporteImpuesto { get; set; }

        // Factura-specific validation
        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(ClaveProdServ);
        }

        // Factura-specific methods
        public string GetTipoImpuesto()
        {
            return Tasa == "Excento" ? "Exento" : "Gravado";
        }
    }
}