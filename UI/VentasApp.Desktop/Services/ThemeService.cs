using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;

namespace VentasApp.Desktop.Services;

public class ThemeService
{
    public void AplicarTema(bool oscuro)
    {
        var dicts = System.Windows.Application.Current.Resources.MergedDictionaries;

        var oldTheme = dicts.FirstOrDefault(d =>
            d.Source?.OriginalString.EndsWith("/Colors.xaml", StringComparison.OrdinalIgnoreCase) == true ||
            d.Source?.OriginalString.EndsWith("/ColorsDark.xaml", StringComparison.OrdinalIgnoreCase) == true);

        if (oldTheme is not null)
            dicts.Remove(oldTheme);

        var newUri = oscuro
            ? new Uri("pack://application:,,,/VentasApp.Desktop;component/Resources/Styles/ColorsDark.xaml")
            : new Uri("pack://application:,,,/VentasApp.Desktop;component/Resources/Styles/Colors.xaml");

        dicts.Add(new ResourceDictionary { Source = newUri });

        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetBaseTheme(oscuro ? BaseTheme.Dark : BaseTheme.Light);
        paletteHelper.SetTheme(theme);
    }
}
