using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ERPMateus.Controls;

public partial class KpiCard : UserControl
{
    public KpiCard()
    {
        InitializeComponent();
    }
    
    public static readonly StyledProperty<string?> IconeProperty =
        AvaloniaProperty.Register<KpiCard, string?>(nameof(Icone), "Home");

    public string? Icone
    {
        get => GetValue(IconeProperty);
        set => SetValue(IconeProperty, value);
    }
    
    public static readonly StyledProperty<string?> TituloProperty =
        AvaloniaProperty.Register<KpiCard, string?>(nameof(Titulo), "Titulo");

    public string? Titulo
    {
        get => GetValue(TituloProperty);
        set => SetValue(TituloProperty, value);
    }
    
    public static readonly StyledProperty<string?> ValorProperty =
        AvaloniaProperty.Register<KpiCard, string?>(nameof(Valor), "Valor");

    public string? Valor
    {
        get => GetValue(ValorProperty);
        set => SetValue(ValorProperty, value);
    }
}