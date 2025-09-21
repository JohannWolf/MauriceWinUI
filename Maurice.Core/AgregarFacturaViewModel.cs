using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Core.Models;
using Maurice.Core.Services;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Maurice.Core
{
    public partial class AgregarFacturaViewModel: ObservableObject
    {
        private readonly IFileService _fileService;

        // Add constructor that accepts IFileService
        public AgregarFacturaViewModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        [ObservableProperty]
        private string _statusMessage = "Bienvenido";
        // Event to request dialog show from View
        public event Func<List<XmlEntry>, Task<ContentDialogResult>> ShowPreviewRequested;

        [RelayCommand]
        public async Task ProcessXmlFileAsync(StorageFile file)
        {
            try
            {
                StatusMessage = "Processing file...";

                // Process XML (delegates to specialized method)
                var result = await _fileService.ProcessXmlFileAsync(file);

                if(result is not null && result.Count > 0)
                {
                    var previewData = result.Select(kv => new XmlEntry
                    {
                        Key = kv.Key,
                        Value = kv.Value
                    }).ToList();

                    // Show dialog via event
                    if (ShowPreviewRequested != null)
                    {
                        var dialogResult = await ShowPreviewRequested(previewData);

                        if (dialogResult == ContentDialogResult.Primary)
                        {
                            await SaveDataAsync(previewData);
                            StatusMessage = $"Archivo procesado: {file.Name}";
                        }
                        else
                        {
                            StatusMessage = "Operación cancelada";
                        }
                    }
                    else
                    {
                        // Fallback: auto-save if no event handler
                        await SaveDataAsync(previewData);
                        StatusMessage = $"Archivo procesado: {file.Name}";
                    }
                } else
                {
                    StatusMessage = "No data found in XML.";
                }
                StatusMessage = $"Processed: {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private async Task SaveDataAsync(List<XmlEntry> data)
        {
            // Your save logic here
            // This could save to database, file, etc.
            await Task.Delay(500); // Simulate save operation
        }
    }
}
