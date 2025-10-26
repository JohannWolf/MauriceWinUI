using CommunityToolkit.Mvvm.ComponentModel;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;
using static Maurice.Data.DatabaseService;

namespace Maurice.Core
{
    public partial class ReporteAnualViewModel : BaseReporteViewModel
    {
        public ReporteAnualViewModel(IDatabaseService databaseService)
            : base(databaseService)
        {
        }
        [ObservableProperty]
        private string _title = "Reporte Anual";
        [ObservableProperty]
        private ObservableCollection<int> _availableYears = new();
        [ObservableProperty]
        private int? _selectedYear;

        protected override async Task<List<Comprobante>> GetReportDataAsync()
        {
            var reportData = new List<Comprobante>();
            try
            {
                // Logic to generate monthly report based on SelectedMonth
                reportData = await _databaseService.SearchComprobantesAsync(date: SearchDate, period: SearchPeriod.Annual);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error generating report: {Message}", ex.Message);
            }
            return reportData;
        }
    }
}
