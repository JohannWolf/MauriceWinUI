using CommunityToolkit.Mvvm.ComponentModel;

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