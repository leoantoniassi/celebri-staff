using CelebriStaff.Views;

namespace CelebriStaff;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
    }
}
