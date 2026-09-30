using CelebriStaff.ViewModels;

namespace CelebriStaff.Views;

public partial class EmailPage : ContentPage
{
    private readonly EmailViewModel _viewModel;

    public EmailPage(EmailViewModel viewModel)
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
