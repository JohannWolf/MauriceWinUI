using Maurice.Core;
using Maurice.Core.Services;
using Maurice.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using Windows.Storage.Pickers;
using WinRT.Interop; // Added for WindowNative

namespace Maurice
{
    public sealed partial class MainWindow : Window
    {
        //public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            // Create the service and pass it to ViewModel
            //IFileService fileService = new FileService();
            //ViewModel = new MainViewModel(fileService);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            //contentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate(typeof(BuscarFactura), new EntranceNavigationTransitionInfo());
        }
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag != null)
            {
                string tag = args.InvokedItemContainer.Tag.ToString();
                Type navPageType = GetPageTypeFromTag(tag);

                if (navPageType != null)
                {
                    NavView_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private Type GetPageTypeFromTag(string tag)
        {
            return tag switch
            {
                "BuscarFactura" => typeof(BuscarFactura),
                "AgregarFactura" => typeof(AgregarFactura),
                "Configuracion" => typeof(Configuracion),
                "ReporteMensual" => typeof(ReporteMensual),
                "ReporteAnual" => typeof(ReporteAnual),
                _ => null
            };
        }
        private void NavView_Navigate(
        Type navPageType,NavigationTransitionInfo transitionInfo)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = contentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                contentFrame.Navigate(navPageType, null, transitionInfo);
            }
        }
    }
}