using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ERPMateus.Controls;

public partial class TopBar : UserControl
{
    public TopBar()
    {
        InitializeComponent();  
    } 

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<TopBar, string>(nameof(Title), "Dashboard");

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<string?> SubtitleProperty =
        AvaloniaProperty.Register<TopBar, string?>(nameof(Subtitle));

    public string? Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly StyledProperty<object?> ActionsProperty =
        AvaloniaProperty.Register<TopBar, object?>(nameof(Actions));

    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }
}