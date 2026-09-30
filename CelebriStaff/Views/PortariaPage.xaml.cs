using CelebriStaff.ViewModels;

namespace CelebriStaff.Views;

public partial class PortariaPage : ContentPage
{
    // Outros porteiros registram entradas ao mesmo tempo; a tela se atualiza sozinha.
    private static readonly TimeSpan IntervaloAtualizacao = TimeSpan.FromSeconds(8);

    private readonly PortariaViewModel _viewModel;
    private IDispatcherTimer? _timer;

    public PortariaPage(PortariaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.AtualizarAsync();

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = IntervaloAtualizacao;
        _timer.Tick += async (_, _) => await _viewModel.AtualizarAsync();
        _timer.Start();
    }

    protected override void OnDisappearing()
    {
        _timer?.Stop();
        _timer = null;
        base.OnDisappearing();
    }
}
