using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CarLogBook.Infrastructure;
using CarLogBook.Infrastructure.Services;
using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Serilog;

namespace CarLogBook.Android
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var appDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CarLogBook");

            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            var dbPath = Path.Combine(appDataDir, "carlogbook.db");
            var logPath = Path.Combine(appDataDir, "logs", "carlogbook-.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("CarLogBook application starting...");
            Log.Information("Database path: {DbPath}", dbPath);

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Logging.AddSerilog(dispose: true);

            Log.Information("Services configuration started");

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddDbContext<CarLogBookDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IFuelEntryRepository, FuelEntryRepository>();
            builder.Services.AddScoped<IFuelStationRepository, FuelStationRepository>();
            builder.Services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();
            builder.Services.AddScoped<IMaintenanceCategoryRepository, MaintenanceCategoryRepository>();
            builder.Services.AddScoped<IMaintenanceEventRepository, MaintenanceEventRepository>();

            builder.Services.AddScoped<IFuelTypeService, FuelTypeService>();
            builder.Services.AddScoped<IFuelStationService, FuelStationService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IFuelEntryService, FuelEntryService>();
            builder.Services.AddScoped<IMaintenanceEventService, MaintenanceEventService>();
            builder.Services.AddScoped<IMaintenanceCategoryService, MaintenanceCategoryService>();
            builder.Services.AddScoped<IXmlImportService, XmlImportService>();
            builder.Services.AddSingleton<ThemeService>();

            Log.Information("Services configuration completed");

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            Log.Information("CarLogBook application started successfully");

            return app;
        }
    }
}
