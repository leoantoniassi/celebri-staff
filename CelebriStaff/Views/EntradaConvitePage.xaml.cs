using CelebriStaff.ViewModels;

namespace CelebriStaff.Views;

public partial class EntradaConvitePage : ContentPage
{
    public EntradaConvitePage(EntradaConviteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
