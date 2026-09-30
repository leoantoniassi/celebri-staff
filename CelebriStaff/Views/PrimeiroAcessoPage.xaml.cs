using CelebriStaff.ViewModels;

namespace CelebriStaff.Views;

public partial class PrimeiroAcessoPage : ContentPage
{
    public PrimeiroAcessoPage(PrimeiroAcessoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
