using Maurice.Core;
using Maurice.Core.Models;
using Maurice.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Maurice.Views
{
    public sealed partial class AgregarFactura : Page
    {
        public AgregarFacturaViewModel ViewModel { get; }
        private readonly SolidColorBrush _defaultDropAreaBackground;
        private readonly SolidColorBrush _dragOverDropAreaBackground;

        /// <summary>
        /// Service to handle file operations
        /// </summary>
        public AgregarFactura()
        {
            InitializeComponent();
            IFileService fileService = new FileService();
            ViewModel = App.Services.GetService<AgregarFacturaViewModel>();
            // Subscribe to the event
            ViewModel.ShowPreviewRequested += ShowPreviewDialogHandler;
            DataContext = ViewModel;
            // Initialize brushes
            _defaultDropAreaBackground = (SolidColorBrush)App.Current.Resources["SubtleFillColorSecondaryBrush"];
            _dragOverDropAreaBackground = new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
        }
        /// <summary>
        /// Handles the click event for the "Select Files" button, allowing the user to select an XML file using a file
        /// picker.
        /// </summary>
        /// <remarks>This method disables the button during the file selection process to prevent multiple
        /// clicks,  and re-enables it after the operation completes. The file picker is initialized to allow the 
        /// selection of XML files and starts in the Documents library. If a file is selected, it is processed 
        /// asynchronously using the associated ViewModel. If the user cancels the operation, a status message  is
        /// updated in the ViewModel.</remarks>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">The event data associated with the click event.</param>
        private async void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            //Disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton.IsEnabled = false;
            //Create a file picker
            var openPicker = new FileOpenPicker();
            //Make the window accessible from the App class.
            var window = App.MainWindow;
            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WindowNative.GetWindowHandle(window);
            // Initialize the file picker with the window handle (HWND).
            InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.FileTypeFilter.Add(".xml");
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                if (ViewModel != null)
                { 
                    // Process the selected file
                    await ViewModel.ProcessXmlFileAsync(file);
                }
            }
            else
            {
                // User cancelled
                if (ViewModel != null)
                {
                    ViewModel.StatusMessage = "File selection cancelled";
                }
            }
            //Re-enable the button
            senderButton.IsEnabled = true;
        }
        /// <summary>
        /// Handles the drag-over event for the page, providing feedback to the user during a drag-and-drop operation.
        /// </summary>
        /// <remarks>This method sets the accepted operation, updates the drag UI caption to
        /// indicate the action, and modifies the background of the drop area to provide visual feedback.</remarks>
        /// <param name="sender">The source of the event, typically the page or UI element where the drag operation is occurring.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> containing data about the drag operation, including the allowed operations
        /// and UI feedback settings.</param>
        private void Page_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Soltar para procesar facturas";
            DropArea.Background = _dragOverDropAreaBackground;
        }
        /// <summary>
        /// Handles the drop event for the page, processing XML files dropped onto the designated area.
        /// </summary>
        /// <remarks>This method checks if the dropped data contains storage items and filters for XML
        /// files. If exactly one XML file is dropped, it is processed asynchronously using the associated
        /// <c>ViewModel</c>. If multiple XML files are dropped, or if no XML files are detected, an appropriate status
        /// message is set in the <c>ViewModel</c>.</remarks>
        /// <param name="sender">The source of the event, typically the page or control where the drop occurred.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> containing data about the drag-and-drop operation.</param>
        private async void Page_Drop(object sender, DragEventArgs e)
        {
            DropArea.Background = _defaultDropAreaBackground;

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                // Retrieve the dropped items
                var items = await e.DataView.GetStorageItemsAsync();
                var files = items.OfType<StorageFile>().Where(f => f.FileType.ToLower() == ".xml").ToList();

                if(files.Count == 1)
                {   
                    if (ViewModel != null)
                    {
                        await ViewModel.ProcessXmlFileAsync(files[0]);
                    }
                }
                else if(files.Count > 1)
                {
                    if (ViewModel != null)
                    {
                        ViewModel.StatusMessage = "Por favor, seleccione un solo archivo XML a la vez.";
                    }
                }
                else
                {
                    if (ViewModel != null)
                    {
                        ViewModel.StatusMessage = "No se detectaron archivos XML. Por favor, intente de nuevo.";
                    }
                }
            }
        }

        // Process multiple files asynchronously, Possible extension for future use
        /*
        private async Task ProcessFilesAsync(IEnumerable<StorageFile> files)
        {
            StatusText.Text = $"Procesando {files.Count()} archivos...";

            // TODO: Call your ViewModel to process each file
            foreach (var file in files)
            {
                // await ViewModel.ProcessXmlFileAsync(file);
            }

            StatusText.Text = "Archivos procesados correctamente";
        }*/
        /// <summary>
        /// Displays a preview dialog for the provided XML data and returns the result of the dialog interaction.
        /// </summary>
        /// <remarks>The dialog is displayed asynchronously and is bound to the provided XML data via its
        /// <see cref="DataContext"/> property. Ensure that the <paramref name="data"/> parameter is not null and
        /// contains valid entries to avoid unexpected behavior.</remarks>
        /// <param name="data">A list of <see cref="XmlEntry"/> objects representing the XML data to be displayed in the dialog.</param>
        /// <returns>A <see cref="ContentDialogResult"/> indicating the result of the dialog interaction. Possible values include
        /// <see cref="ContentDialogResult.Primary"/>, <see cref="ContentDialogResult.Secondary"/>, or <see
        /// cref="ContentDialogResult.None"/>.</returns>
        private async Task<ContentDialogResult> ShowPreviewDialogHandler(List<XmlEntry> data)
        {
            var dialog = new XmlPreviewDialog();
            dialog.DataContext = new { XmlData = data };
            dialog.XamlRoot = this.XamlRoot;

            return await dialog.ShowAsync();
        }

        // Clean up event subscription
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ShowPreviewRequested -= ShowPreviewDialogHandler;
            base.OnNavigatedFrom(e);
        }
    }
}