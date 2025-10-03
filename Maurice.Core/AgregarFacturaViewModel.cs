using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Core.Models;
using Maurice.Core.Services;
using Maurice.Data.Models;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Maurice.Core
{
    public partial class AgregarFacturaViewModel : ObservableObject
    {
        private readonly IFileService _fileService;

        public AgregarFacturaViewModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        [ObservableProperty]
        private string _statusMessage = "Arrastra o selecciona archivos XML";

        [ObservableProperty]
        private Comprobante _currentComprobante;

        // Event to request dialog show from View
        public event Func<ComprobanteDisplay, Task<ContentDialogResult>> ShowPreviewRequested;

        [RelayCommand]
        public async Task ProcessXmlFileAsync(StorageFile file)
        {
            try
            {
                StatusMessage = "Procesando archivo...";

                // Process XML - now returns Comprobante instead of Dictionary
                var comprobante = await _fileService.ProcessXmlFileAsync(file);

                if (comprobante != null)
                {
                    // Store the current comprobante
                    CurrentComprobante = comprobante;
                    var displayModel = comprobante.ToDisplayModel(); // Use extension method

                    // Show dialog via event
                    if (ShowPreviewRequested != null)
                    {
                        var dialogResult = await ShowPreviewRequested(displayModel);

                        if (dialogResult == ContentDialogResult.Primary)
                        {
                            await SaveDataAsync(comprobante);
                            StatusMessage = GetSuccessMessage(comprobante);
                        }
                        else
                        {
                            StatusMessage = "Operación cancelada";
                            CurrentComprobante = null;
                        }
                    }
                    else
                    {
                        // Fallback: auto-save if no event handler
                        await SaveDataAsync(comprobante);
                        StatusMessage = GetSuccessMessage(comprobante);
                    }
                }
                else
                {
                    StatusMessage = "No se pudo procesar el archivo XML";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                CurrentComprobante = null;
            }
        }

        private async Task SaveDataAsync(Comprobante comprobante)
        {
            try
            {
                // TODO: Implement your save logic here
                // This could save to database, file system, etc.

                if (comprobante is Factura factura)
                {
                    // Save factura specific data
                    await SaveFacturaAsync(factura);
                }
                else if (comprobante is Nomina nomina)
                {
                    // Save nomina specific data
                    await SaveNominaAsync(nomina);
                }

                // Clear current comprobante after successful save
                CurrentComprobante = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar: {ex.Message}";
                throw;
            }
        }

        private async Task SaveFacturaAsync(Factura factura)
        {
            // Your factura save logic here
            await Task.Delay(100); // Simulate save operation
        }

        private async Task SaveNominaAsync(Nomina nomina)
        {
            // Your nomina save logic here
            await Task.Delay(100); // Simulate save operation
        }

        private string GetSuccessMessage(Comprobante comprobante)
        {
            if (comprobante is Factura factura)
            {
                return $"Factura {factura.Folio} guardada correctamente - Total: {factura.Total:C}";
            }
            else if (comprobante is Nomina nomina)
            {
                return $"Nómina {nomina.Folio} guardada correctamente - Neto: {nomina.GetNetoAPagar():C}";
            }

            return $"Documento {comprobante.Folio} guardado correctamente";
        }
    }
}
