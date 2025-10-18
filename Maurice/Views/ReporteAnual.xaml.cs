using Maurice.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views
{
    /// <summary>
    /// NEeed to adjust reports to avoid repetitive code
    /// </summary>
    public sealed partial class ReporteAnual : Page
    {
        public ReporteAnualViewModel ViewModel { get; }
        public ReporteAnual()
        {
            InitializeComponent();
            PopulateYears();
            ViewModel = App.Services.GetService<ReporteAnualViewModel>();
            DataContext = ViewModel;
        }
        private void PopulateYears()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 10; year--)
            {
                YearComboBox.Items.Add(year.ToString());
            }
        }
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedYear = GetSelectedMonth();
            if (selectedYear.HasValue)
            {
                ViewModel.SearchDate = selectedYear.Value;
                ViewModel.GenerateReportCommand.ExecuteAsync(null);
            }
        }
        private DateTime? GetSelectedMonth()
        {
            if (YearComboBox.SelectedItem is string yearText &&
            int.TryParse(yearText, out int year))
            {
                return new DateTime(year);
            }
            return null;
        }
        //Generate report for current month by default
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.GenerateReportCommand.ExecuteAsync(null);
        }
    }
}
