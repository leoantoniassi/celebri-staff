namespace CelebriStaff.Views.Controls;

public partial class BrandHero : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(BrandHero), string.Empty,
        propertyChanged: (b, _, n) => ((BrandHero)b).TitleView.Text = n as string);

    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(
        nameof(Subtitle), typeof(string), typeof(BrandHero), string.Empty,
        propertyChanged: (b, _, n) =>
        {
            var hero = (BrandHero)b;
            hero.SubtitleView.Text = n as string;
            hero.SubtitleView.IsVisible = !string.IsNullOrEmpty(n as string);
        });


    /// <summary>Logo do buffet; quando informada, substitui o emblema Celebri.</summary>
    public static readonly BindableProperty LogoUrlProperty = BindableProperty.Create(
        nameof(LogoUrl), typeof(string), typeof(BrandHero), null,
        propertyChanged: (b, _, n) =>
        {
            var hero = (BrandHero)b;
            var url = n as string;
            var has = !string.IsNullOrEmpty(url);
            hero.LogoView.Source = has ? ImageSource.FromUri(new Uri(url!)) : null;
            hero.LogoView.IsVisible = has;
            hero.Emblem.IsVisible = !has;
        });

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Subtitle { get => (string)GetValue(SubtitleProperty); set => SetValue(SubtitleProperty, value); }
    public string? LogoUrl { get => (string?)GetValue(LogoUrlProperty); set => SetValue(LogoUrlProperty, value); }

    public BrandHero()
    {
        InitializeComponent();
    }
}
