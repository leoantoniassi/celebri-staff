using CelebriStaff.ViewModels;
using CelebriStaff.Views;

namespace CelebriStaff;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("primeiro-acesso", typeof(PrimeiroAcessoPage));

        Routing.RegisterRoute("portaria", typeof(PortariaPage));
        Routing.RegisterRoute("portaria-leitor", typeof(LeitorQrPage));
        Routing.RegisterRoute("portaria-entrada", typeof(EntradaConvitePage));
        EscalaCardViewModel.ModulosDisponiveis.Add("portaria");

        Routing.RegisterRoute("garcom", typeof(GarcomPage));
        Routing.RegisterRoute("garcom-mesa", typeof(MesaPage));
        Routing.RegisterRoute("cozinha", typeof(CozinhaPage));
        EscalaCardViewModel.ModulosDisponiveis.Add("garcom");
        EscalaCardViewModel.ModulosDisponiveis.Add("cozinha");
    }
}
