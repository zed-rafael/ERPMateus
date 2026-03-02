using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velopack;

namespace ERPMateus.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string server;
    [ObservableProperty] private string _UpdateStatus;
    [ObservableProperty] private bool _IsUpdating;

    public void TestCommand() 
    {
        // Aqui entraria sua lógica de ADO.NET ou Entity Framework
        UpdateStatus = $"Conectando em {Server}...";
    }

    [RelayCommand]
    public async Task CheckUpdateCommand()
    {
        IsUpdating = true;
        UpdateStatus = "Buscando atualizações...";
        
        try 
        {
            var mgr = new UpdateManager("https://seu-servidor-de-updates.com/releases");
            var newVersion = await mgr.CheckForUpdatesAsync();
            
            if (newVersion == null) {
                UpdateStatus = "Você já está na última versão.";
            } else {
                UpdateStatus = "Baixando atualização...";
                await mgr.DownloadUpdatesAsync(newVersion);
                UpdateStatus = "Pronto! Reinicie para aplicar.";
                // mgr.ApplyUpdatesAndRestart(); // Opcional: força o reinício
            }
        }
        catch { UpdateStatus = "Erro ao buscar update."; }
        finally { IsUpdating = false; }
    }
}