using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Maurice.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReporteMensual
{
    public ReporteMensualViewModel ViewModel { get; }
    public ReporteMensual()
    {
        InitializeComponent();
        PopulateMonths();
        ViewModel = App.Services.GetService<ReporteMensualViewModel>();
        DataContext = ViewModel;
    }
    private void PopulateMonths()
    {
        MonthComboBox.Items.Clear();
        for (int month = 1; month <= 12; month++)
        {
            var monthName = new DateTime(1, month, 1).ToString("MMMM");
            var comboBoxItem = new ComboBoxItem
            {
                //Capitalize first letter of the month
                Content = char.ToUpper(monthName[0]) + monthName.Substring(1),
                Tag = month // Store the month number in the Tag property
            };
            MonthComboBox.Items.Add(comboBoxItem);
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
        if (MonthComboBox.SelectedItem is ComboBoxItem monthItem &&
            int.TryParse(monthItem.Tag.ToString(), out int month))
        {
            return new DateTime(year, month, 1);
        }
        return null;
    }
    //Generate report for current month by default
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.GenerateReportCommand.ExecuteAsync(null);
    }
}
