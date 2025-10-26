using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using Microsoft.UI.Xaml.Controls;
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
        private decimal _totalIVA;
        [ObservableProperty]
        private int _numberOfRecords;

        // Common commands
        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Generando reporte...";
                ReportResult.Clear();

                // Template method - derived classes implement this
                var results = await GetReportDataAsync();

                foreach (var item in results)
                {
                    ReportResult.Add(item);
                }

                CalculateAmounts(ReportResult);
                NumberOfRecords = ReportResult.Count;
                StatusMessage = $"Reporte generado: {NumberOfRecords} comprobantes";
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
            TotalIVA = 0;
            StatusMessage = "Seleccione el período";
        }

        // Template method - derived classes must implement
        protected abstract Task<List<Comprobante>> GetReportDataAsync();

        // Common calculations
        protected void CalculateAmounts(ICollection<Comprobante> reportResult)
        {
            TotalExpenses = reportResult.Sum(c => c.GetExpenseAmount());
            TotalIncomes = reportResult.Sum(c => c.GetIncomeAmount());
            TotalISRRetenido = reportResult.Sum(c => c.GetISRAmount());
            TotalIVA = reportResult.Sum(c => c.GetIVAAmount());
        }
    }
}
