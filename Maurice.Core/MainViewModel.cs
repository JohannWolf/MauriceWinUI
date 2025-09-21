using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maurice.Core.Models;
using Maurice.Core.Services;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace Maurice.Core
{
    public partial class MainViewModel : ObservableObject
    {
        // Add constructor that accepts IFileService
        public MainViewModel()
        {
        }

        [ObservableProperty]
        private string _statusMessage = "Bienvenido";

        public event Action<string> OpenWindowRequested;
    }
}