using System.Windows.Input;

namespace CelebriStaff.Views.Controls;

public partial class EmptyStateView : ContentView
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(EmptyStateView), string.Empty,
        propertyChanged: (b, _, n) => ((EmptyStateView)b).IconView.Text = n as string);

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(EmptyStateView), string.Empty,
        propertyChanged: (b, _, n) => ((EmptyStateView)b).TitleView.Text = n as string);

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message), typeof(string), typeof(EmptyStateView), string.Empty,
        propertyChanged: (b, _, n) => ((EmptyStateView)b).MessageView.Text = n as string);

    public static readonly BindableProperty ActionTextProperty = BindableProperty.Create(
        nameof(ActionText), typeof(string), typeof(EmptyStateView), string.Empty,
        propertyChanged: (b, _, n) =>
        {
            var view = (EmptyStateView)b;
            view.ActionButton.Text = n as string;
            view.ActionButton.IsVisible = !string.IsNullOrEmpty(n as string);
        });

    public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(
        nameof(ActionCommand), typeof(ICommand), typeof(EmptyStateView), null,
        propertyChanged: (b, _, n) => ((EmptyStateView)b).ActionButton.Command = n as ICommand);

    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Message { get => (string)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }
    public string ActionText { get => (string)GetValue(ActionTextProperty); set => SetValue(ActionTextProperty, value); }
    public ICommand? ActionCommand { get => (ICommand?)GetValue(ActionCommandProperty); set => SetValue(ActionCommandProperty, value); }

    public EmptyStateView()
    {
        InitializeComponent();
    }
}
