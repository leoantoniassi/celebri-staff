using System.Windows.Input;

namespace CelebriStaff.Views.Controls;

public partial class QuantityStepper : ContentView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(int), typeof(QuantityStepper), 0,
        propertyChanged: (b, _, n) => ((QuantityStepper)b).ValueLabel.Text = n?.ToString());

    public static readonly BindableProperty MinusCommandProperty = BindableProperty.Create(
        nameof(MinusCommand), typeof(ICommand), typeof(QuantityStepper), null,
        propertyChanged: (b, _, n) => ((QuantityStepper)b).MinusButton.Command = n as ICommand);

    public static readonly BindableProperty PlusCommandProperty = BindableProperty.Create(
        nameof(PlusCommand), typeof(ICommand), typeof(QuantityStepper), null,
        propertyChanged: (b, _, n) => ((QuantityStepper)b).PlusButton.Command = n as ICommand);

    public int Value { get => (int)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public ICommand? MinusCommand { get => (ICommand?)GetValue(MinusCommandProperty); set => SetValue(MinusCommandProperty, value); }
    public ICommand? PlusCommand { get => (ICommand?)GetValue(PlusCommandProperty); set => SetValue(PlusCommandProperty, value); }

    public QuantityStepper()
    {
        InitializeComponent();
        ValueLabel.Text = Value.ToString();
    }
}
