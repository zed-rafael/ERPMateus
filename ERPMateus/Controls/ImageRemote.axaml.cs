using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ERPMateus.Controls;

public partial class ImageRemote : UserControl
{
    private static readonly HttpClient _http = new HttpClient();
    
    public ImageRemote()
    {
        InitializeComponent();
        Initialized += async (_, __) => await LoadAsync();
    }
    
    public static readonly StyledProperty<string> UrlProperty =
        AvaloniaProperty.Register<ImageRemote, string>(nameof(Url));

    public string Url
    {
        get => GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }

    public static readonly StyledProperty<Bitmap?> ImageSourceProperty =
        AvaloniaProperty.Register<ImageRemote, Bitmap?>(nameof(ImageSource));

    public Bitmap? ImageSource
    {
        get => GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<ImageRemote, Stretch>(nameof(Stretch), Stretch.UniformToFill);

    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    // -----------------------------
    // MÉTODO PRINCIPAL (CARREGAR)
    // -----------------------------
    private async Task LoadAsync()
    {
        Console.WriteLine("Rodando foto: ");
        if (string.IsNullOrWhiteSpace(Url))
        {
            LoadFallback();
            return;
        }

        try
        {
            var response = await _http.GetAsync(Url);

            // Se falhou (404, 500 etc) → usa fallback
            if (!response.IsSuccessStatusCode)
            {
                LoadFallback();
                return;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;

            // Se não for imagem → usa fallback
            if (contentType is null ||
                !(contentType.StartsWith("image")))
            {
                LoadFallback();
                return;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            ImageSource = new Bitmap(stream);
        }
        catch
        {
            // Erro de rede, DNS, timeout, etc.
            LoadFallback();
        }
    }
    
    private void LoadFallback()
    {
        try
        {
            var uri = new Uri("avares://MasterDetail/Assets/avatar-user.png");

            using var stream = AssetLoader.Open(uri);
            ImageSource = new Bitmap(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao carregar imagem local: " + ex.Message);
        }
    }
}