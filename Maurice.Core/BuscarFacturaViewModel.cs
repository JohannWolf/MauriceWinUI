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
                    UUID = "FAC-001-2024",
                    Folio = "001",
                    Date = new DateTime(2024, 1, 15),
                    Total = 1250.50m,
                    Status = "Activa"
                },
                new Invoice {
                    UUID = "FAC-002-2024",
                    Folio = "002",
                    Date = new DateTime(2024, 1, 18),
                    Total = 890.75m,
                    Status = "Cancelada"
                },
                new Invoice {
                    UUID = "FAC-003-2024",
                    Folio = "003",
                    Date = new DateTime(2024, 2, 5),
                    Total = 2345.00m,
                    Status = "Activa"
                },
                new Invoice {
                    UUID = "FAC-004-2024",
                    Folio = "004",
                    Date = new DateTime(2024, 2, 12),
                    Total = 567.30m,
                    Status = "Pendiente"
                },
                new Invoice {
                    UUID = "FAC-005-2024",
                    Folio = "005",
                    Date = new DateTime(2024, 2, 20),
                    Total = 1789.45m,
                    Status = "Activa"
                }
            };
        }
    }
}

public class Invoice
{
    public string UUID { get; set; }
    public string Folio { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
}