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

        public decimal TotalImpuestosRetenidos { get; set; }
        public decimal TotalOtrasDeducciones { get; set; }
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
    }
}
