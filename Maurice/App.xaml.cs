using Maurice.Core;
using Maurice.Core.Services;
using Maurice.Data;
using Maurice.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maurice
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow MainWindow { get; private set; }
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
            InitializeComponent();
        }
        /// <summary>
        /// Services setup for Dependency injection
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // Register DbContext with the connection string
            services.AddDbContext<MauriceDbContext>(options =>
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = System.IO.Path.Combine(path, "maurice.db");
                options.UseSqlite($"Data Source={dbPath}");
            });
            services.AddSingleton<IFileService, Maurice.Core.Services.FileService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddTransient<AgregarFacturaViewModel>();
            services.AddTransient<ConfiguracionViewModel>();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Initialize database
            var dbService = Services.GetService<IDatabaseService>();
            await dbService.InitializeDatabaseAsync();

            MainWindow = new MainWindow();
            MainWindow.Activate();
        }
    }
}
