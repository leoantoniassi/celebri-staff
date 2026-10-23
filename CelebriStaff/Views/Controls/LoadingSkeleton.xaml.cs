namespace CelebriStaff.Views.Controls;

public partial class LoadingSkeleton : ContentView
{
    private const string PulseAnimation = "skeleton-pulse";

    public LoadingSkeleton()
    {
        InitializeComponent();
        SemanticProperties.SetDescription(this, "Carregando");
    }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == IsVisibleProperty.PropertyName) UpdatePulse();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        UpdatePulse();
    }

    // A animação só roda enquanto o skeleton está visível e anexado à tela.
    private void UpdatePulse()
    {
        this.AbortAnimation(PulseAnimation);
        Host.Opacity = 1;

        if (!IsVisible || Handler is null) return;

        // Cosseno em um ciclo completo: começa e termina em 1.0, então o loop não "pula".
        new Animation(v => Host.Opacity = 0.725 + 0.275 * Math.Cos(v * 2 * Math.PI), 0, 1)
            .Commit(this, PulseAnimation, length: 1400, repeat: () => true);
    }
}
