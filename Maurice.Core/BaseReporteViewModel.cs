using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;

namespace Maurice.Core
{
    public abstract partial class BaseReporteViewModel: ObservableObject
    {
        protected readonly IDatabaseService _databaseService;

        protected BaseReporteViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Common properties
        [ObservableProperty]
        private ObservableCollection<Comprobante> _reportResult = new();
        [ObservableProperty]
        private DateTime _searchDate = DateTime.Now;
        [ObservableProperty]
        private string _statusMessage = "Seleccione el período";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private decimal _totalExpenses;

        [ObservableProperty]
        private decimal _totalIncomes;

        [ObservableProperty]
        private decimal _totalISRRetenido;

        [ObservableProperty]
        private decimal _totalIVAPorPagar;
        [ObservableProperty]
        private decimal _totalIVAPagado;
        [ObservableProperty]
        public decimal _netAmount;
        [ObservableProperty]
        public decimal _IsDeductibleAmount;
        [ObservableProperty]
        private int _numberOfFacturas;
        [ObservableProperty]
        private decimal _amountFacturas;
        [ObservableProperty]
        private int _numberOfNominas;
        [ObservableProperty]
        private decimal _amountNominas;

        // Common commands
        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Generando reporte...";
                ClearReport();

                // Template method - derived classes implement this
                var results = await GetReportDataAsync();

                foreach (var item in results)
                {
                    ReportResult.Add(item);
                }

                CalculateAmounts(ReportResult);
                StatusMessage = $"Reporte generado exitosamente";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ClearReport()
        {
            ReportResult.Clear();
            TotalExpenses = 0;
            TotalIncomes = 0;
            TotalISRRetenido = 0;
            TotalIVAPorPagar = 0;
            TotalIVAPagado = 0;
            NumberOfFacturas = 0;
            AmountFacturas = 0;
            NumberOfNominas = 0;
            AmountNominas = 0;
            StatusMessage = "Seleccione el período";
        }

        // Template method - derived classes must implement
        protected abstract Task<List<Comprobante>> GetReportDataAsync();

        // Common calculations
        protected void CalculateAmounts(ICollection<Comprobante> reportResult)
        {
            foreach (var comprobante in reportResult)
            {
                TotalExpenses += comprobante.GetExpenseAmount();
                TotalIncomes += comprobante.GetIncomeAmount();
                TotalISRRetenido += comprobante.GetISRAmount();

                var ivaAmount = comprobante.GetIVAAmount();

                switch (comprobante.TipoDeDocumento)
                {
                    case "Factura":
                        if (comprobante.TipoDeTransaccion == 1)
                        {
                            TotalIVAPorPagar += ivaAmount;
                            NumberOfFacturas++;
                            AmountFacturas += comprobante.GetIncomeAmount();
                        }
                        else if (comprobante.TipoDeTransaccion == 2)
                            TotalIVAPagado += ivaAmount;
                        break;
                    case "Nomina":
                        NumberOfNominas++;
                        AmountNominas += comprobante.GetIncomeAmount();
                        break;
                }
            }
        }
    }
}
