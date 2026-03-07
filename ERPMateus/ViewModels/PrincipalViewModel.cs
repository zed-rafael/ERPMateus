using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Models;
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
    [ObservableProperty] private bool _isMenuAberto = true;
    [ObservableProperty] private string _server;
    [ObservableProperty] private int _pagina;
    [ObservableProperty] private string _updateStatus = "Atualizar";
    [ObservableProperty] private bool _isUpdating;

    [ObservableProperty] private Usuario? _usuario = Global.InfoAplicacao.Usuario;

    public PrincipalViewModel()
    {
        Pagina =  (int)PaginaAplicacao.Dashboard;
    }

    public void TestCommand() 
    {
        UpdateStatus = $"Conectando em {Server}...";
    }

    [RelayCommand]
    private void AbrirFecharMenu()
    {
        IsMenuAberto = !IsMenuAberto;
    }
    
    // [RelayCommand]
    // private void Sair()
    // {
    //     Usuario = null;
    //     Pagina =  (int)PaginaAplicacao.Lo;
    // }
    
    

    [RelayCommand]
    private async Task CheckUpdate()
    {
        try
        {
            var mgr = new UpdateManager("https://meusttr.net/updates/erpmateus/linux/");
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