using Maurice.Core;
using Maurice.Views.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views
{
    /// <summary>
    /// Need to adjust reports to avoid repetitive code
    /// </summary>
    public sealed partial class ReporteAnual : Page
    {
        public ReporteAnualViewModel ViewModel { get; }
        public ReporteAnual()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<ReporteAnualViewModel>();
            DataContext = ViewModel;
            PopulateYears();
        }

        //Generate report for current month by default
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Auto-generate report for current year
            ViewModel.GenerateReportCommand.ExecuteAsync(null);
        }
        //Date Pickers
        private void PopulateYears()
        {
            ViewModel.AvailableYears.Clear();
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 10; year--)
            {
                ViewModel.AvailableYears.Add(year);
            }
        }
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedYear = GetSelectedYear();
            if (selectedYear.HasValue)
            {
                ViewModel.SearchDate = selectedYear.Value;
                ViewModel.GenerateReportCommand.ExecuteAsync(null);
            }
        }
        private DateTime? GetSelectedYear()
        {
            if (ViewModel.SelectedYear.HasValue)
            {
                return new DateTime(ViewModel.SelectedYear.Value,1,1);
            }
            return null;
        }
    }
}
