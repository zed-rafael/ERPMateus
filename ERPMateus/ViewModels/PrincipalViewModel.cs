using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;
using Velopack;

namespace ERPMateus.ViewModels;

public enum PaginaAplicacao
{
    Dashboard = 0,
    Configurações = 1,
    Correções = 2
}

public partial class PrincipalViewModel : ViewModelBase
{
    public DashboardViewModel DashboardViewModel { get; } = new();
    
    public string AppVersion => $"v{AppInfo.Version}";
    [ObservableProperty] private string server;
    [ObservableProperty] private int pagina;
    [ObservableProperty] private string _UpdateStatus = "Atualizar";
    [ObservableProperty] private bool _IsUpdating;


    public PrincipalViewModel()
    {
        Pagina =  (int)PaginaAplicacao.Dashboard;
    }

    public void TestCommand() 
    {
        UpdateStatus = $"Conectando em {Server}...";
    }
    
    [RelayCommand]
    private async Task CheckUpdate()
    {
        try
        {
            var mgr = new UpdateManager("https://meusttr.net/updates/erpmateus/");
            var newVersion = await mgr.CheckForUpdatesAsync();

            if (newVersion == null)
            {
                UpdateStatus = "Você já está na última versão.";
            }
            else
            {
                UpdateStatus = "Baixando atualização...";
                await mgr.DownloadUpdatesAsync(newVersion);
                UpdateStatus = "Pronto! Reinicie para aplicar.";
                mgr.ApplyUpdatesAndRestart(newVersion);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus = "Erro na atualização!";
        }
        finally { IsUpdating = false; }
    }
}