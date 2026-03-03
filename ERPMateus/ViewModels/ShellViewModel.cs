using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;

namespace ERPMateus.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    public DashboardViewModel DashboardViewModel { get; } = new();
    
    private readonly UpdateService _updates;

    [ObservableProperty] private ViewModelBase current;
    [ObservableProperty] private string status = "Pronto.";
    public string AppVersion => $"v{AppInfo.Version}";

    public ConnectionsViewModel Connections { get; }

    public ShellViewModel()
    {
        _updates = new UpdateService("https://meusttr.net/updates/erpmateus");
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