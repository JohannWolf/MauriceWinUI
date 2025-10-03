using Maurice.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Core.Models
{
    public static class ComprobanteDisplayExtensions
    {
        public static ComprobanteDisplay ToDisplayModel(this Comprobante comprobante)
        {
            if (comprobante == null) return null;

            var display = new ComprobanteDisplay
            {
                Tipo = comprobante.TipoDeDocumento,
                Folio = comprobante.Folio ?? "N/A",
                Fecha = comprobante.Fecha?.ToString("dd/MM/yyyy") ?? "N/A",
                UUID = comprobante.UUID ?? "N/A",
                Emisor = FormatEmisor(comprobante.RfcEmisor, comprobante.NombreEmisor),
                Receptor = comprobante.RfcReceptor ?? "N/A",
                Total = comprobante.Total.ToString("C2")
            };

            AddCommonFields(display, comprobante);

            if (comprobante is Factura factura)
            {
                AddFacturaFields(display, factura);
            }
            else if (comprobante is Nomina nomina)
            {
                AddNominaFields(display, nomina);
            }

            // Group fields by category for better organization
            var groupedFields = display.Campos
                .GroupBy(f => f.Categoria)
                .SelectMany(g => new[]
                {
            new DisplayField { Nombre = g.Key, Valor = "", Categoria = "Header" }
                }.Concat(g))
                .ToList();

            display.Campos = groupedFields;

            return display;
        }

        private static string FormatEmisor(string rfc, string nombre)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(rfc))
                return $"{nombre} ({rfc})";
            return rfc ?? nombre ?? "N/A";
        }

        private static void AddCommonFields(ComprobanteDisplay display, Comprobante comprobante)
        {
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "SubTotal", Valor = comprobante.SubTotal.ToString("C2"), Categoria = "Totales" },
                new DisplayField { Nombre = "Descuento", Valor = comprobante.Descuento.ToString("C2"), Categoria = "Totales" },
                new DisplayField { Nombre = "Total", Valor = comprobante.Total.ToString("C2"), Categoria = "Totales" },
                new DisplayField { Nombre = "Fecha Emisión", Valor = comprobante.Fecha?.ToString("dd/MM/yyyy HH:mm"), Categoria = "Generales" },
                new DisplayField { Nombre = "UUID", Valor = comprobante.UUID ?? "N/A", Categoria = "Generales" }
            });
        }

        private static void AddFacturaFields(ComprobanteDisplay display, Factura factura)
        {
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Clave Prod/Serv", Valor = factura.ClaveProdServ ?? "N/A", Categoria = "Concepto" },
                new DisplayField { Nombre = "Descripción", Valor = factura.Descripcion ?? "N/A", Categoria = "Concepto" },
                new DisplayField { Nombre = "Base", Valor = factura.Base.ToString("C2"), Categoria = "Impuestos" },
                new DisplayField { Nombre = "Tasa", Valor = factura.Tasa ?? "Exento", Categoria = "Impuestos" },
                new DisplayField { Nombre = "Importe Impuesto", Valor = factura.ImporteImpuesto.ToString("C2"), Categoria = "Impuestos" }
            });
        }

        private static void AddNominaFields(ComprobanteDisplay display, Nomina nomina)
        {
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Total Gravado", Valor = nomina.TotalGravado.ToString("C2"), Categoria = "Percepciones" },
                new DisplayField { Nombre = "Total Exento", Valor = nomina.TotalExento.ToString("C2"), Categoria = "Percepciones" },
                new DisplayField { Nombre = "Total Percepciones", Valor = nomina.TotalPercepciones.ToString("C2"), Categoria = "Percepciones" },
                new DisplayField { Nombre = "Total Impuestos Retenidos", Valor = nomina.TotalImpuestosRetenidos.ToString("C2"), Categoria = "Deducciones" },
                new DisplayField { Nombre = "Total Otras Deducciones", Valor = nomina.TotalOtrasDeducciones.ToString("C2"), Categoria = "Deducciones" },
                new DisplayField { Nombre = "Total Deducciones", Valor = nomina.TotalDeducciones.ToString("C2"), Categoria = "Deducciones" },
                new DisplayField { Nombre = "Neto a Pagar", Valor = nomina.GetNetoAPagar().ToString("C2"), Categoria = "Calculados" }
            });
        }
    }
}
