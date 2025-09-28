using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BuscarFactura
{
    public BuscarFactura()
    {
        InitializeComponent();
        PopulateYears();
    }
    private void SelectAll_Checked(object sender, RoutedEventArgs e)
    {
        DateOptionCheckBox.IsChecked = RfcOptionCheckBox.IsChecked = true;
        MonthComboBox.IsEnabled = YearComboBox.IsEnabled = RfcTextBox.IsEnabled = true;
    }

    private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
    {
        DateOptionCheckBox.IsChecked = RfcOptionCheckBox.IsChecked = false;
        MonthComboBox.IsEnabled = YearComboBox.IsEnabled = RfcTextBox.IsEnabled = false;
    }

    private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
    {
        // If the SelectAll box is checked (all options are selected), 
        // clicking the box will change it to its indeterminate state.
        // Instead, we want to uncheck all the boxes,
        // so we do this programatically. The indeterminate state should
        // only be set programatically, not by the user.

        if (DateOptionCheckBox.IsChecked == true &&
            RfcOptionCheckBox.IsChecked == true)
        {
            // This will cause SelectAll_Unchecked to be executed, so
            // we don't need to uncheck the other boxes here.
            OptionsAllCheckBox.IsChecked = false;
        }
    }

    private void SetCheckedState()
    {
        // Controls are null the first time this is called, so we just 
        // need to perform a null check on any one of the controls.
        if (DateOptionCheckBox != null)
        {
            if (DateOptionCheckBox.IsChecked == true &&
                RfcOptionCheckBox.IsChecked == true)
            {
                OptionsAllCheckBox.IsChecked = true;
            }
            else if (DateOptionCheckBox.IsChecked == false &&
                RfcOptionCheckBox.IsChecked == false)
            {
                OptionsAllCheckBox.IsChecked = false;
            }
            else
            {
                // Set third state (indeterminate) by setting IsChecked to null.
                OptionsAllCheckBox.IsChecked = null;
            }
        }
    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        SetCheckedState();

        if (sender == DateOptionCheckBox)
        {
            MonthComboBox.IsEnabled = YearComboBox.IsEnabled = true;
        }
        else if (sender == RfcOptionCheckBox)
        {
            RfcTextBox.IsEnabled = true;
        }
    }

    private void Option_Unchecked(object sender, RoutedEventArgs e)
    {
        SetCheckedState();

        if (sender == DateOptionCheckBox)
        {
            MonthComboBox.IsEnabled = YearComboBox.IsEnabled = false;
        }
        else if (sender == RfcOptionCheckBox)
        {
            RfcTextBox.IsEnabled = false;
        }
    }

    private void PopulateYears()
    {
        int currentYear = DateTime.Now.Year;
        for (int year = currentYear; year >= currentYear - 10; year--)
        {
            YearComboBox.Items.Add(year.ToString());
        }
    }

    // Get selected date
    private DateTime? GetSelectedDate()
    {
        if (MonthComboBox.SelectedItem is ComboBoxItem monthItem &&
            YearComboBox.SelectedItem is string yearText &&
            int.TryParse(yearText, out int year) &&
            int.TryParse(monthItem.Tag.ToString(), out int month))
        {
            return new DateTime(year, month, 1);
        }
        return null;
    }
}
