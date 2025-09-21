using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Maurice.Core.Services
{
    public class FileService : IFileService
    {
        public async Task<IDictionary<string, string>> ProcessXmlFileAsync(StorageFile file)
        {
            var result = new Dictionary<string, string>();

            try
            {
                // Read the XML content as a string
                string xmlContent = await FileIO.ReadTextAsync(file);

                // Parse the XML content using XDocument
                XDocument doc = XDocument.Parse(xmlContent);

                // Extract data
                var comprobante = doc.Root;
                if (comprobante != null)
                {
                    result["Tipo de Documento"] = TipoDeDocumento(comprobante);
                    result["Folio"] = comprobante.Attribute("Folio")?.Value;
                    result["Fecha"] = comprobante.Attribute("Fecha")?.Value;

                    var complemento = comprobante.Element(XName.Get("Complemento", "http://www.sat.gob.mx/cfd/4"));
                    if (complemento != null)
                    {
                        var timbreFiscalDigital = complemento.Element(XName.Get("TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital")); if (timbreFiscalDigital != null)
                            result["UUID"] = timbreFiscalDigital.Attribute("UUID")?.Value ?? "NA"; // Default value for UUID
                    }

                    var emisor = comprobante.Element(XName.Get("Emisor", "http://www.sat.gob.mx/cfd/4"));
                    if (emisor != null)
                    {
                        result["RFC Emisor"] = emisor.Attribute("Rfc")?.Value;
                        result["Nombre de Emisor"] = emisor.Attribute("Nombre")?.Value;
                    }

                    var receptor = comprobante.Element(XName.Get("Receptor", "http://www.sat.gob.mx/cfd/4"));
                    if (receptor != null)
                    {
                        result["RFC Receptor"] = receptor.Attribute("Rfc")?.Value;
                    }
                    //Datos de Facturas
                    var concepto = comprobante.Descendants(XName.Get("Concepto", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                    if (concepto != null)
                    {
                        var value = concepto.Attribute("ClaveProdServ")?.Value;

                        if (value == "85121600")
                        {
                            result["Descripcion"] = "Honorarios medicos y gastos hospitalarios";
                        }
                    }

                    var impuestos = comprobante.Element(XName.Get("Impuestos", "http://www.sat.gob.mx/cfd/4"));
                    if (impuestos != null)
                    {
                        var traslado = impuestos.Descendants(XName.Get("Traslado", "http://www.sat.gob.mx/cfd/4")).FirstOrDefault();
                        if (traslado != null)
                        {
                            result["Base"] = traslado.Attribute("Base")?.Value ?? "0.00"; // Default value for Base
                            result["Tasa"] = traslado.Attribute("TasaOCuota")?.Value ?? "Excento";
                            result["Importe de impuesto"] = traslado.Attribute("Importe")?.Value ?? "0.00"; // Default value for Importe
                        }
                    }

                    //Datos Nominas
                    if(result["Tipo de Documento"] == "Nomina")
                    {
                        var complementoNomina = complemento.Element(XName.Get("Nomina", "http://www.sat.gob.mx/nomina12"));
                        if (complementoNomina != null)
                        {
                            var percepciones = complementoNomina.Element(XName.Get("Percepciones", "http://www.sat.gob.mx/nomina12"));
                            if (percepciones != null)
                            {
                                result["Percepciones"] = percepciones.Attribute("TotalGravado")?.Value;
                                result["Percepciones Exentas"] = percepciones.Attribute("TotalExento")?.Value;
                            }
                            var deducciones = complementoNomina.Element(XName.Get("Deducciones", "http://www.sat.gob.mx/nomina12"));
                            if (deducciones != null)
                            {
                                result["Deducciones"] = deducciones.Attribute("TotalImpuestosRetenidos")?.Value;
                                result["Otros Descuentos"] = deducciones.Attribute("TotalOtrasDeducciones")?.Value;
                            }
                            result["Total de Percepciones"] = comprobante.Attribute("SubTotal")?.Value;
                            result["Total de Deducciones"] = comprobante.Attribute("Descuento")?.Value;
                        }
                    }

                    result["Total Pagado"] = comprobante.Attribute("Total")?.Value;
                }

                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new ApplicationException("Error processing XML file", ex);
            }
        }

        private string TipoDeDocumento(XElement comprobante)
        {
            string result = comprobante.Attribute("TipoDeComprobante")?.Value == "N" ? "Nomina" :
                "Factura";
            //Implementae logica por ingreso o egreso
            return result;
        }
    }
}