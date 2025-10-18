using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;

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
                var reportData = await _databaseService.SearchComprobantesAsync(date: date);
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
            decimal totalExpense = 0;
            decimal totalIncome = 0;
            decimal totalIsr = 0;
            decimal totalIVA = 0;
            foreach (var item in reportResult)
            {
                if (item.TipoDeTransaccion == 1) // Income
                {
                    totalIncome += item.Total;
                    if (item is Nomina nomina)
                    {
                        totalIsr += nomina.TotalImpuestosRetenidos;
                    }
                    else if (item is Factura factura)
                    {
                        totalIVA += factura.RetencionImpuesto;
                    }
                }
                else if (item.TipoDeTransaccion == 2) // Expense
                {
                    totalExpense += item.Total;
                    if (item is Factura factura)
                    {
                        totalIVA += factura.ImporteImpuesto;
                    }
                }
            }
            TotalExpenses = totalExpense;
            TotalIncomes = totalIncome;
            TotalISRRetenido = totalIsr;
            TotalIVA = totalIVA;
        }
    }
}
