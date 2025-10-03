using System;
using System.Linq;
using System.Xml.Linq;
using Windows.Storage;
using Maurice.Data.Models;

namespace Maurice.Core.Services
{
    public class FileService : IFileService
    {
        public async Task<Comprobante> ProcessXmlFileAsync(StorageFile file)
        {
            try
            {
                // Read the XML content as a string
                string xmlContent = await FileIO.ReadTextAsync(file);

                // Parse the XML content using XDocument
                XDocument doc = XDocument.Parse(xmlContent);

                // Extract data and create appropriate model
                var comprobante = doc.Root;
                if (comprobante != null)
                {
                    var documentType = TipoDeDocumento(comprobante);

                    if (documentType == "Nomina")
                    {
                        return CreateNominaFromXml(comprobante, file.Name);
                    }
                    else
                    {
                        return CreateFacturaFromXml(comprobante, file.Name);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error processing XML file", ex);
            }
        }

        private Factura CreateFacturaFromXml(XElement comprobante, string fileName)
        {
            var factura = new Factura
            {
                TipoDeDocumento = "Factura",
                Folio = comprobante.Attribute("Folio")?.Value,
                Fecha = ParseDateTime(comprobante.Attribute("Fecha")?.Value),
                SubTotal = ParseDecimal(comprobante.Attribute("SubTotal")?.Value),
                Descuento = ParseDecimal(comprobante.Attribute("Descuento")?.Value),
                Total = ParseDecimal(comprobante.Attribute("Total")?.Value),
                FileName = fileName
            };

            var complemento = comprobante.Element(XName.Get("Complemento", "http://www.sat.gob.mx/cfd/4"));
            if (complemento != null)
            {
                var timbreFiscalDigital = complemento.Element(XName.Get("TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital"));
                if (timbreFiscalDigital != null)
                    factura.UUID = timbreFiscalDigital.Attribute("UUID")?.Value ?? "NA";
            }

            var emisor = comprobante.Element(XName.Get("Emisor", "http://www.sat.gob.mx/cfd/4"));
            if (emisor != null)
            {
                factura.RfcEmisor = emisor.Attribute("Rfc")?.Value;
                factura.NombreEmisor = emisor.Attribute("Nombre")?.Value;
            }

            var receptor = comprobante.Element(XName.Get("Receptor", "http://www.sat.gob.mx/cfd/4"));
            if (receptor != null)
            {
                factura.RfcReceptor = receptor.Attribute("Rfc")?.Value;
            }

            // Datos de Facturas
            var concepto = comprobante.Descendants(XName.Get("Concepto", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
            if (concepto != null)
            {
                var value = concepto.Attribute("ClaveProdServ")?.Value;
                factura.ClaveProdServ = value;

                if (value == "85121600")
                {
                    factura.Descripcion = "Honorarios medicos y gastos hospitalarios";
                }
            }

            var impuestos = comprobante.Element(XName.Get("Impuestos", "http://www.sat.gob.mx/cfd/4"));
            if (impuestos != null)
            {
                var traslado = impuestos.Descendants(XName.Get("Traslado", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                if (traslado != null)
                {
                    factura.Base = ParseDecimal(traslado.Attribute("Base")?.Value ?? "0.00");
                    factura.Tasa = traslado.Attribute("TasaOCuota")?.Value ?? "Excento";
                    factura.ImporteImpuesto = ParseDecimal(traslado.Attribute("Importe")?.Value ?? "0.00");
                }
            }

            return factura;
        }

        private Nomina CreateNominaFromXml(XElement comprobante, string fileName)
        {
            var nomina = new Nomina
            {
                TipoDeDocumento = "Nomina",
                Folio = comprobante.Attribute("Folio")?.Value,
                Fecha = ParseDateTime(comprobante.Attribute("Fecha")?.Value),
                SubTotal = ParseDecimal(comprobante.Attribute("SubTotal")?.Value),
                Descuento = ParseDecimal(comprobante.Attribute("Descuento")?.Value),
                Total = ParseDecimal(comprobante.Attribute("Total")?.Value),
                FileName = fileName
            };

            var complemento = comprobante.Element(XName.Get("Complemento", "http://www.sat.gob.mx/cfd/4"));
            if (complemento != null)
            {
                var timbreFiscalDigital = complemento.Element(XName.Get("TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital"));
                if (timbreFiscalDigital != null)
                    nomina.UUID = timbreFiscalDigital.Attribute("UUID")?.Value ?? "NA";

                // Datos Nominas
                var complementoNomina = complemento.Element(XName.Get("Nomina", "http://www.sat.gob.mx/nomina12"));
                if (complementoNomina != null)
                {
                    var percepciones = complementoNomina.Element(XName.Get("Percepciones", "http://www.sat.gob.mx/nomina12"));
                    if (percepciones != null)
                    {
                        nomina.TotalGravado = ParseDecimal(percepciones.Attribute("TotalGravado")?.Value);
                        nomina.TotalExento = ParseDecimal(percepciones.Attribute("TotalExento")?.Value);
                    }

                    var deducciones = complementoNomina.Element(XName.Get("Deducciones", "http://www.sat.gob.mx/nomina12"));
                    if (deducciones != null)
                    {
                        nomina.TotalImpuestosRetenidos = ParseDecimal(deducciones.Attribute("TotalImpuestosRetenidos")?.Value);
                        nomina.TotalOtrasDeducciones = ParseDecimal(deducciones.Attribute("TotalOtrasDeducciones")?.Value);
                    }

                    nomina.TotalPercepciones = nomina.SubTotal;
                    nomina.TotalDeducciones = nomina.Descuento;
                }
            }

            var emisor = comprobante.Element(XName.Get("Emisor", "http://www.sat.gob.mx/cfd/4"));
            if (emisor != null)
            {
                nomina.RfcEmisor = emisor.Attribute("Rfc")?.Value;
                nomina.NombreEmisor = emisor.Attribute("Nombre")?.Value;
            }

            var receptor = comprobante.Element(XName.Get("Receptor", "http://www.sat.gob.mx/cfd/4"));
            if (receptor != null)
            {
                nomina.RfcReceptor = receptor.Attribute("Rfc")?.Value;
            }

            return nomina;
        }

        private string TipoDeDocumento(XElement comprobante)
        {
            return comprobante.Attribute("TipoDeComprobante")?.Value == "N" ? "Nomina" : "Factura";
        }

        private DateTime? ParseDateTime(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime result))
                return result;
            return null;
        }

        private decimal ParseDecimal(string decimalString)
        {
            if (decimal.TryParse(decimalString, out decimal result))
                return result;
            return 0m;
        }
    }
}