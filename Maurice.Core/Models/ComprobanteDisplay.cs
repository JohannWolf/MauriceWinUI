using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Core.Models
{
    public class ComprobanteDisplay
    {
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string UUID { get; set; }
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string Total { get; set; }
        public List<DisplayField> Campos { get; set; } = new List<DisplayField>();
    }
}