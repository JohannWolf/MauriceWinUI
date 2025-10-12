using CommunityToolkit.Mvvm.ComponentModel;
using Maurice.Data;
using Microsoft.EntityFrameworkCore.Storage;
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
    }
}