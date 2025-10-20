using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;
using static Maurice.Data.DatabaseService;

namespace Maurice.Core
{
    public partial class ReporteAnualViewModel: ObservableObject
    {
        private readonly IDatabaseService _databaseService;

        public ReporteAnualViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private DateTime _searchDate = DateTime.Now;
        [ObservableProperty]
        private ObservableCollection<Comprobante> _reportData = new();
        [ObservableProperty]
        private bool _isGeneratingReport;
        [ObservableProperty]
        private string _statusMessage = "Listo para generar reporte";
        [ObservableProperty]
        private decimal _totalIncomes;
        [ObservableProperty]
        private decimal _totalExpenses;
        [ObservableProperty]
        private int _numberOfRecords;
        [ObservableProperty]
        private decimal _totalISRRetenido;
        [ObservableProperty]
        private decimal _totalIVA;

        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            IsGeneratingReport = true;
            StatusMessage = "Generando reporte mensual...";
            try
            {
                // Logic to generate monthly report based on SelectedMonth
                var date = SearchDate;
                var reportData = await _databaseService.SearchComprobantesAsync(date: date, period: SearchPeriod.Annual);
                // Process reportData as needed for the report
                ReportData.Clear();
                foreach (var item in reportData)
                {
                    ReportData.Add(item);
                }
                NumberOfRecords = ReportData.Count;
                CalculateAmounts(ReportData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error generating report: {Message}", ex.Message);
            }
            finally
            {
                IsGeneratingReport = false;
                StatusMessage = "Reporte generado.";
            }
        }
        private void CalculateAmounts(ICollection<Comprobante> reportResult)
        {
            TotalExpenses = reportResult.Sum(c => c.GetExpenseAmount());
            TotalIncomes = reportResult.Sum(c => c.GetIncomeAmount());
            TotalISRRetenido = reportResult.Sum(c => c.GetISRAmount());
            TotalIVA = reportResult.Sum(c => c.GetIVAAmount());
        }
    }
}
