using System;
using System.IO;
using System.Text.Json;

namespace VentasApp.Desktop.Services;

public class AppSettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "VentasApp", "settings.json");

    public bool ModoOscuro { get; set; }
    public int StockMinimoLibreria { get; set; } = 5;
    public int StockMinimoUniforme { get; set; } = 3;

    public void Guardar()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this));
    }

    public static AppSettingsService Cargar()
    {
        if (!File.Exists(SettingsPath))
            return new AppSettingsService();
        try
        {
            return JsonSerializer.Deserialize<AppSettingsService>(
                File.ReadAllText(SettingsPath)) ?? new AppSettingsService();
        }
        catch
        {
            return new AppSettingsService();
        }
    }
}
