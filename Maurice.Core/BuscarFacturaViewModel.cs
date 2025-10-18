using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;

namespace Maurice.Core
{
    public partial class BuscarFacturaViewModel : ObservableObject
    {
        private readonly IDatabaseService _databaseService;

        public BuscarFacturaViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string _statusMessage = "Listo para buscar";
        [ObservableProperty]
        private string? _searchRfc;
        [ObservableProperty]
        private DateTime? _searchDate;
        [ObservableProperty]
        private bool _isSearching;
        [ObservableProperty]
        private ObservableCollection<Comprobante> _searchResults = new();

        [RelayCommand]
        private async Task SearchAsync()
        {
            try
            {
                IsSearching = true;
                StatusMessage = "Buscando facturas...";
                //Call database service to get results based on criteria
                var results = await _databaseService.SearchComprobantesAsync(
                    rfc: SearchRfc,
                    date: SearchDate
                    );
                SearchResults.Clear();
                foreach (var item in results)
                {
                    SearchResults.Add(item);
                }
                StatusMessage = results.Count > 0
                    ? $"{results.Count} resultados encontrados."
                    : "No se encontraron resultados.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error durante la búsqueda: {ex.Message}";
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}