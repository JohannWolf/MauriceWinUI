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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReporteAnual : Page
    {
        public ReporteAnual()
        {
            InitializeComponent();
            PopulateYears();
        }
        private void PopulateYears()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 10; year--)
            {
                YearComboBox.Items.Add(year.ToString());
            }
        }
    }
}
