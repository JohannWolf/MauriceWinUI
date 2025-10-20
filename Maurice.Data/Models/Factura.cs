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
        //Traslado -> Importe = IVA
        public decimal ImporteImpuesto { get; set; }
        //Retencion -> Importe = ISR Retenido
        public decimal RetencionImpuesto { get; set; }

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
        public override decimal GetIncomeAmount()
            => TipoDeTransaccion == 1 ? Total : 0;

        public override decimal GetExpenseAmount()
            => TipoDeTransaccion == 2 ? Total : 0;

        public override decimal GetIVAAmount()
            => ImporteImpuesto;

        public override decimal GetISRAmount()
            => RetencionImpuesto;
    }
}