using System.ComponentModel;
using CelebriStaff.Models;

namespace CelebriStaff.Services.Theme;

/// <summary>
/// Aplica a identidade visual (cores + logo) do buffet contratante em tempo
/// de execução. As cores viram Application.Current.Resources, referenciadas
/// nas telas via {DynamicResource ColorPrimary} etc — atualizam a UI
/// imediatamente, sem precisar reiniciar o app. O LogoUrl fica exposto por
/// INotifyPropertyChanged para telas com {Binding} num Image.
/// </summary>
public class ThemeService : INotifyPropertyChanged
{
    private string? _logoUrl;
    private string _nomeFantasia = "Celebri Staff";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? LogoUrl
    {
        get => _logoUrl;
        private set
        {
            _logoUrl = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogoUrl)));
        }
    }

    public string NomeFantasia
    {
        get => _nomeFantasia;
        private set
        {
            _nomeFantasia = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomeFantasia)));
        }
    }

    public void Apply(TenantConfig tenant)
    {
        var resources = Application.Current?.Resources;
        if (resources is null) return;

        resources["ColorPrimary"] = Color.FromArgb(tenant.Cores.Primaria);
        resources["ColorSecondary"] = Color.FromArgb(tenant.Cores.Secundaria);
        resources["ColorTertiary"] = Color.FromArgb(tenant.Cores.Terciaria);

        LogoUrl = tenant.LogoUrl;
        NomeFantasia = tenant.NomeFantasia;
    }
}
