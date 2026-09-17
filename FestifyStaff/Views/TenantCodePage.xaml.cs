using FestifyStaff.ViewModels;

namespace FestifyStaff.Views;

public partial class TenantCodePage : ContentPage
{
    private readonly TenantCodeViewModel _viewModel;

    public TenantCodePage(TenantCodeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RestoreSessionIfAnyAsync();
    }
}
