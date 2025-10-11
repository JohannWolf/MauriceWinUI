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
        private User _currentUser = new User();

        [ObservableProperty]
        private string _statusMessage = "Cargando...";

        [ObservableProperty]
        private bool _isUserConfigured;

        [ObservableProperty]
        private bool _isLoading = true;

        [RelayCommand]
        private async Task LoadUserAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Cargando configuración...";

                var user = await _databaseService.GetUserAsync();
                if (user != null)
                {
                    CurrentUser = user;
                    IsUserConfigured = true;
                    StatusMessage = "Usuario cargado correctamente";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar usuario: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                StatusMessage = IsUserConfigured ? "Configuración lista" : "Por favor, configure su usuario";
            }
        }
        [RelayCommand]
        private async Task SaveUserAsync()
        {
            if (!CanSaveUser()) return; 
            try
            {
                IsLoading = true;
                StatusMessage = "Guardando usuario...";

                await _databaseService.SaveUserAsync(CurrentUser);
                IsUserConfigured = true;
                StatusMessage = "Usuario guardado correctamente";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar usuario: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanSaveUser()
        {
            if (IsLoading) return false;
            if (CurrentUser == null) return false;

            if (string.IsNullOrWhiteSpace(CurrentUser?.FirstName) &&
                   string.IsNullOrWhiteSpace(CurrentUser?.LastName) &&
                   string.IsNullOrWhiteSpace(CurrentUser?.Rfc)) {
                StatusMessage = "Todos los campos son requeridos";
                return false;
            }
            if(CurrentUser.Rfc.Length != 13)
            {
                StatusMessage = "El RFC debe tener 13 caracteres";
                return false;
            }
            return true;
        }
    }
}