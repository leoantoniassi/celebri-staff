using CelebriStaff.ViewModels;

namespace CelebriStaff.Views;

public partial class MesaPage : ContentPage
{
    private readonly MesaViewModel _viewModel;
    private IDispatcherTimer? _timer;

    public MesaPage(MesaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.AtualizarAsync();
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(8);
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
