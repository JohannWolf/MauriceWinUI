using Maurice.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.Graphics;

namespace Maurice
{
    public sealed partial class MainWindow : Window
    {
        //public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            // Get the window handle
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);

            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            // Set the desired size using Resize()

            appWindow.Resize(new SizeInt32(1200, 900)); // Set width to 800 pixels and height to 600 pixels
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