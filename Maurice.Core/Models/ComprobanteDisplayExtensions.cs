using Maurice.Data.Models;

namespace Maurice.Core.Models
{
    public static class ComprobanteDisplayExtensions
    {
        // Desired category order
        private static readonly List<string> CategoryOrder = new List<string>
        {
            "Generales",
            "Concepto",
            "Percepciones",
            "Deducciones",
            "Impuestos",
            "Totales"
        };

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

            // Add fields in the desired order
            AddGeneralFields(display, comprobante);

            if (comprobante is Factura factura)
            {
                AddFacturaFields(display, factura);
            }
            else if (comprobante is Nomina nomina)
            {
                AddNominaFields(display, nomina);
            }

            AddTotalFields(display, comprobante);

            return display;
        }

        private static string FormatEmisor(string rfc, string nombre)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(rfc))
                return $"{nombre} ({rfc})";
            return rfc ?? nombre ?? "N/A";
        }

        private static void AddGeneralFields(ComprobanteDisplay display, Comprobante comprobante)
        {
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Tipo de Documento", Valor = comprobante.TipoDeDocumento, Categoria = "Generales" },
                new DisplayField { Nombre = "Folio", Valor = comprobante.Folio ?? "N/A", Categoria = "Generales" },
                new DisplayField { Nombre = "Fecha Emisión", Valor = comprobante.Fecha?.ToString("dd/MM/yyyy HH:mm") ?? "N/A", Categoria = "Generales" },
                new DisplayField { Nombre = "UUID", Valor = comprobante.UUID ?? "N/A", Categoria = "Generales" },
                new DisplayField { Nombre = "RFC Emisor", Valor = comprobante.RfcEmisor ?? "N/A", Categoria = "Generales" },
                new DisplayField { Nombre = "Nombre Emisor", Valor = comprobante.NombreEmisor ?? "N/A", Categoria = "Generales" },
                new DisplayField { Nombre = "RFC Receptor", Valor = comprobante.RfcReceptor ?? "N/A", Categoria = "Generales" }
            });
        }

        private static void AddFacturaFields(ComprobanteDisplay display, Factura factura)
        {
            // Concepto fields
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Clave Prod/Serv", Valor = factura.ClaveProdServ ?? "N/A", Categoria = "Concepto" },
                new DisplayField { Nombre = "Descripción", Valor = factura.Descripcion ?? "N/A", Categoria = "Concepto" }
            });

            // Impuestos fields
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Base", Valor = factura.Base.ToString("C2"), Categoria = "Impuestos" },
                new DisplayField { Nombre = "Tasa", Valor = factura.Tasa ?? "Exento", Categoria = "Impuestos" },
                new DisplayField { Nombre = "Importe Impuesto", Valor = factura.ImporteImpuesto.ToString("C2"), Categoria = "Impuestos" }
            });
        }

        private static void AddNominaFields(ComprobanteDisplay display, Nomina nomina)
        {
            // Percepciones fields
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Total Gravado", Valor = nomina.TotalGravado.ToString("C2"), Categoria = "Percepciones" },
                new DisplayField { Nombre = "Total Exento", Valor = nomina.TotalExento.ToString("C2"), Categoria = "Percepciones" },
                new DisplayField { Nombre = "Total Percepciones", Valor = nomina.TotalPercepciones.ToString("C2"), Categoria = "Percepciones" }
            });

            // Deducciones fields
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "Total Impuestos Retenidos", Valor = nomina.TotalImpuestosRetenidos.ToString("C2"), Categoria = "Deducciones" },
                new DisplayField { Nombre = "Total Otras Deducciones", Valor = nomina.TotalOtrasDeducciones.ToString("C2"), Categoria = "Deducciones" },
                new DisplayField { Nombre = "Total Deducciones", Valor = nomina.TotalDeducciones.ToString("C2"), Categoria = "Deducciones" }
            });
        }

        private static void AddTotalFields(ComprobanteDisplay display, Comprobante comprobante)
        {
            // Always add total fields last
            display.Campos.AddRange(new[]
            {
                new DisplayField { Nombre = "SubTotal", Valor = comprobante.SubTotal.ToString("C2"), Categoria = "Totales" },
                new DisplayField { Nombre = "Total", Valor = comprobante.Total.ToString("C2"), Categoria = "Totales" }
            });

            // Add calculated fields for nómina
            if (comprobante is Nomina nomina)
            {
                display.Campos.Add(new DisplayField
                {
                    Nombre = "Neto a Pagar",
                    Valor = nomina.GetNetoAPagar().ToString("C2"),
                    Categoria = "Totales"
                });
            }
        }

        //Method to get fields in the correct grouped order
        public static List<DisplayField> GetOrderedFields(this ComprobanteDisplay display)
        {
            if (display?.Campos == null) return new List<DisplayField>();

            return display.Campos
                .OrderBy(f => CategoryOrder.IndexOf(f.Categoria))
                .ThenBy(f => f.Nombre)
                .ToList();
        }
    }
}
