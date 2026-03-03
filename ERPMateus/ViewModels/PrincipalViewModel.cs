using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;
using Velopack;

namespace ERPMateus.ViewModels;

public partial class PrincipalViewModel : ViewModelBase
{
    public DashboardViewModel DashboardViewModel { get; } = new();
    
    public string AppVersion => $"v{AppInfo.Version}";
    [ObservableProperty] private string server;
    [ObservableProperty] private string _UpdateStatus = "Atualizar";
    [ObservableProperty] private bool _IsUpdating;

    public void TestCommand() 
    {
        UpdateStatus = $"Conectando em {Server}...";
    }
    
    [RelayCommand]
    public async Task CheckUpdate()
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