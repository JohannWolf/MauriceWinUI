using Maurice.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using static Maurice.Core.ReporteMensualViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReporteMensual: Page
{
    public ReporteMensualViewModel ViewModel { get; }
    public ReporteMensual()
    {
        InitializeComponent();
        ViewModel = App.Services.GetService<ReporteMensualViewModel>();
        DataContext = ViewModel;
        PopulateMonths();
    }
    private void PopulateMonths()
    {
        ViewModel.AvailableMonths.Clear();
        for (int month = 1; month <= 12; month++)
        {
            var monthName = new DateTime(1, month, 1).ToString("MMMM");
            string capitalizedMonthName = char.ToUpper(monthName[0]) + monthName.Substring(1);
            ViewModel.AvailableMonths.Add(new MonthItem { Name = capitalizedMonthName, Value = month });
        }
    }
    private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedMonth = GetSelectedMonth();
        if (selectedMonth.HasValue)
        {
            ViewModel.SearchDate = selectedMonth.Value;
            ViewModel.GenerateReportCommand.ExecuteAsync(null);
        }
    }
    private DateTime? GetSelectedMonth()
    {
        int year = DateTime.Now.Year;
        if (ViewModel.SelectedMonth.HasValue)
        {
            return new DateTime(year,ViewModel.SelectedMonth.Value, 1);
        }
        return null;
    }
    //Generate report for current month by default
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.GenerateReportCommand.ExecuteAsync(null);
    }
}
