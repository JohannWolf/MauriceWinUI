using Maurice.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Configuracion : Page
    {
        public ConfiguracionViewModel ViewModel { get; }
        public Configuracion()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<ConfiguracionViewModel>();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Auto-load user data when page is navigated to
            ViewModel.LoadUserCommand.ExecuteAsync(null);
        }
        private void MainButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsInEditMode) { ViewModel.IsInEditMode = true; }
            else
            {
                ViewModel.SaveUserCommand.ExecuteAsync(null);
            }
        }
        // Handles the Click event on the Button on the page and opens the Popup.
        private void ShowPopupOffset(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already and on EditMode
            if (!ListPopup.IsOpen && ViewModel.IsInEditMode) { ListPopup.IsOpen = true; }
        }
        // Handles the Seleccionar Button inside the Popup control and sets the Regimen.
        private void SelectRegimenClicked(object sender, RoutedEventArgs e)
        {
            // get the selected Regimen from the Button's DataContext
            if (RegimenFiscalListBox.SelectedItems.Count > 0)
            {
                ViewModel.CurrentUser?.RegimenesFiscales.Clear();
                ViewModel.SelectedRegimenesFiscales.Clear();
                foreach (var item in RegimenFiscalListBox.SelectedItems)
                {
                    if (item is Data.Models.RegimenFiscal selectedRegimen)
                    {
                        ViewModel.SelectedRegimenesFiscales.Add(selectedRegimen);
                    }
                }
                // Set New values
                ViewModel.RegimenFiscalInputText = string.Join(", ", ViewModel.SelectedRegimenesFiscales.Select(i => i.Name));

            }
            // close the Popup
            if (ListPopup.IsOpen) { ListPopup.IsOpen = false; }
        }
        // Handles the Cancel Button inside the Popup control and closes the Popup.
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it
            if (ListPopup.IsOpen) { ListPopup.IsOpen = false; }
        }

        private void OnCancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.IsInEditMode)
            {
                ViewModel.IsInEditMode = false;
                return;
            }
            // Navigate back to the previous page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
