using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice.Views.Shared
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class BaseReporte : UserControl
    {
        public BaseReporte()
        {
            InitializeComponent();
        }
        // Property to set the period selection content
        public UIElement PeriodSelection
        {
            get => PeriodSelectionContent.Content as UIElement;
            set => PeriodSelectionContent.Content = value;
        }
    }
}
