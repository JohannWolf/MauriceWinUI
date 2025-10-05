using Maurice.Core;
using Maurice.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

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

        public void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var user = new User
            {
                FirstName = NameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Rfc = RFCTextBox.Text,
            };
            _ = ViewModel.SaveUserData(user);
        }
    }
}
