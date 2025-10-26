using CommunityToolkit.Mvvm.ComponentModel;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;
using static Maurice.Data.DatabaseService;

namespace Maurice.Core
{
    public partial class ReporteMensualViewModel: BaseReporteViewModel
    {
        public ReporteMensualViewModel(IDatabaseService databaseService)
            : base(databaseService)
        {
        }
        [ObservableProperty]
        private string _title = "Reporte Mensual";
        [ObservableProperty]
        private ObservableCollection<MonthItem> _availableMonths = new();
        [ObservableProperty]
        private int? _selectedMonth;

        protected override async Task<List<Comprobante>> GetReportDataAsync()
        {
            var reportData = new List<Comprobante>();
            try
            {
                // Logic to generate monthly report based on SelectedMonth
                reportData = await _databaseService.SearchComprobantesAsync(date: SearchDate, period: SearchPeriod.Monthly);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error generating report: {Message}", ex.Message);
            }
            return reportData;
        }
        // Helper class for months
        public class MonthItem
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }
    }
}
