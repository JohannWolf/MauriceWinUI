using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;

namespace Maurice.Core
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        private readonly IDatabaseService _databaseService;
        public ConfiguracionViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        [ObservableProperty]
        public string _statusMessage = "";

        [RelayCommand]
        public async Task SaveUserData(User user)
        {
            try
            {
                await _databaseService.SaveUserAsync(user);
                StatusMessage = "Datos guardados correctamente";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar datos: {ex.Message}";
            }
        }
    }
}
