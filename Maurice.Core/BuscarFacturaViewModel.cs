using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Maurice.Core
{
    public partial class BuscarFacturaViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Invoice> _testData;

        public BuscarFacturaViewModel()
        {
            // Initialize with some test data
            TestData = new ObservableCollection<Invoice>
            {
                new Invoice {
                    RfcEmisor = "FACA010120N01",
                    Folio = "001",
                    Date = new DateTime(2024, 1, 15),
                    Total = 1250.50m,
                    Impuesto = 150.50m,
                },
                new Invoice {
                    RfcEmisor = "GODE561231GR8",
                    Folio = "002",
                    Date = new DateTime(2024, 2, 20),
                    Total = 3000.00m,
                    Impuesto = 480.00m,
                },
                new Invoice {
                    RfcEmisor = "XEXX010101000",
                    Folio = "003",
                    Date = new DateTime(2024, 3, 5),
                    Total = 750.75m,
                    Impuesto = 120.75m,
                },
                new Invoice {
                    RfcEmisor = "BADD110",
                    Folio = "004",
                    Date = new DateTime(2024, 3, 5),
                    Total = 750.75m,
                    Impuesto = 120.75m,
                }
            };
        }
    }
}

public class Invoice
{
    public string RfcEmisor { get; set; }
    public string Folio { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public decimal Impuesto { get; set; }
}