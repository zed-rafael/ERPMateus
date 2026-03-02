using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;

namespace ERPMateus.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    private readonly UpdateService _updates;

    [ObservableProperty] private ViewModelBase current;
    [ObservableProperty] private string status = "Pronto.";

    public ConnectionsViewModel Connections { get; }

    public ShellViewModel()
    {
        _updates = new UpdateService("https://SEU_FEED_VELOPACK_AQUI");
        Connections = new ConnectionsViewModel(SetStatus);

        Current = Connections;
    }

    private void SetStatus(string msg) => Status = msg;

    [RelayCommand]
    private void GoConnections() => Current = Connections;

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        Status = "Verificando atualização...";
        var res = await _updates.TryUpdateAsync();
        Status = res.Status;
    }
}