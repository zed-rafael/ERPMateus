using CommunityToolkit.Mvvm.ComponentModel;
using ERPMateus.Models;

namespace ERPMateus;

public static class Global
{
    public static string ConnectionString =
        "Data Source=200.98.168.184;Initial Catalog=fetaema_siga_uol;User ID=sa;Password=J@nj@n1001;TrustServerCertificate=True;Encrypt=False;";
    public static InfoAplicacao InfoAplicacao { get; set; } = new InfoAplicacao();
}

public partial class InfoAplicacao : ObservableObject
{
    [ObservableProperty] private Usuario? _usuario;
}