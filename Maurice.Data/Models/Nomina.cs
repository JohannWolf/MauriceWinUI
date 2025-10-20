using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Data.Models
{
    public class Nomina : Comprobante
    {
        // Nomina-specific fields
        public decimal TotalGravado { get; set; }
        public decimal TotalExento { get; set; }
        public decimal TotalPercepciones { get; set; }
        //TotalImpuestosRetenidos = ISR A Cargo
        public decimal TotalImpuestosRetenidos { get; set; }
        //IMSS, INFONAVIT, SAR, etc.
        public decimal TotalOtrasDeducciones { get; set; }
        // TotalDeducciones = TotalImpuestosRetenidos + TotalOtrasDeducciones
        public decimal TotalDeducciones { get; set; }

        // Nomina-specific validation
        public override bool IsValid()
        {
            return base.IsValid() && TotalPercepciones > 0;
        }

        // Nomina-specific methods
        public decimal GetNetoAPagar()
        {
            return TotalPercepciones - TotalDeducciones;
        }
        public override decimal GetIncomeAmount()
         => TipoDeTransaccion == 1 ? Total : 0;

        public override decimal GetExpenseAmount() 
            => 0; // Nominas are income only in my context

        public override decimal GetIVAAmount()
            => 0; // No IVA in Nominas

        public override decimal GetISRAmount()
            => TotalImpuestosRetenidos;
    }
}
