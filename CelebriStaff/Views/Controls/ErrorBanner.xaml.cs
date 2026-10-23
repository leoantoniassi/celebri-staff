namespace CelebriStaff.Views.Controls;

public partial class ErrorBanner : ContentView
{
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message), typeof(string), typeof(ErrorBanner), string.Empty,
        propertyChanged: (b, _, n) =>
        {
            var banner = (ErrorBanner)b;
            var text = n as string;
            banner.MessageView.Text = text;
            banner.IsVisible = !string.IsNullOrEmpty(text);
        });

    public string Message { get => (string)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }

    public ErrorBanner()
    {
        InitializeComponent();
    }
}
