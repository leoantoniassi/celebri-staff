using FestifyStaff.Views;

namespace FestifyStaff;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
    }
}
