using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Data;
using Maurice.Data.Models;
using System.Collections.ObjectModel;

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
        private User? _currentUser = new User();
        [ObservableProperty]
        private string _statusMessage = "Cargando...";
        [ObservableProperty]
        private string _regimenFiscalInputText = "Seleccione un régimen fiscal";
        [ObservableProperty]
        private bool _isUserConfigured = false;
        [ObservableProperty]
        private bool _isInEditMode = false;
        [ObservableProperty]
        private bool _isLoading = true;
        [ObservableProperty]
        private ObservableCollection<RegimenFiscal> _availableRegimenesFiscales = [];
        [ObservableProperty]
        private ObservableCollection<RegimenFiscal> _selectedRegimenesFiscales = [];
        public string SaveButtonText
        {
            get
            {
                if (!IsUserConfigured&&IsInEditMode) return "GUARDAR";
                if (!IsUserConfigured) return "CONFIGURAR";
                return IsInEditMode ? "ACTUALIZAR" : "EDITAR";
            }
        }
        public bool CanSaveOrEdit => !IsLoading;
        partial void OnIsUserConfiguredChanged(bool value)
        {
            OnPropertyChanged(nameof(SaveButtonText));
        }
        partial void OnIsInEditModeChanged(bool value)
        {
            OnPropertyChanged(nameof(SaveButtonText));
        }
        partial void OnIsLoadingChanged(bool value)
        {
            OnPropertyChanged(nameof(CanSaveOrEdit));
            OnPropertyChanged(nameof(SaveButtonText));
        }
        [RelayCommand]
        private async Task LoadUserAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Cargando configuración...";

                var user = await _databaseService.GetUserAsync();
                var regimenes = await _databaseService.GetRegimenesFiscalesAsync();

                foreach (var regimen in regimenes)
                {
                    AvailableRegimenesFiscales.Add(regimen);
                }

                if (user != null)
                {
                    CurrentUser = user;
                    RegimenFiscalInputText = string.Join(", ", CurrentUser.RegimenesFiscales.Select(r => r.Name));
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
                IsInEditMode = false;
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
                foreach (var regimen in SelectedRegimenesFiscales)
                {
                    // Avoid duplicates
                    if (!CurrentUser!.RegimenesFiscales.Any(r => r.Id == regimen.Id))
                    {
                        CurrentUser!.RegimenesFiscales.Add(regimen);
                    }
                }

                await _databaseService.SaveUserAsync(CurrentUser);
                IsUserConfigured = true;
                StatusMessage = "Usuario guardado correctamente";
                IsInEditMode = false;
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

            if (string.IsNullOrWhiteSpace(CurrentUser?.FirstName) ||
                   string.IsNullOrWhiteSpace(CurrentUser?.LastName) ||
                   string.IsNullOrWhiteSpace(CurrentUser?.Rfc)) {
                StatusMessage = "Todos los campos son requeridos";
                return false;
            }
            if(CurrentUser.Rfc.Length != 13)
            {
                StatusMessage = "El RFC debe tener 13 caracteres";
                return false;
            }
            if (SelectedRegimenesFiscales.Count == 0)
            {
                StatusMessage = "Seleccione al menos un régimen fiscal";
                return false;
            }
            return true;
        }
    }
}