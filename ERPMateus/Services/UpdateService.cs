using System;
using System.Threading;
using System.Threading.Tasks;
using ERPMateus.ViewModels.Dto;
using Velopack;

namespace ERPMateus.Services;

public sealed class UpdateService
{
    private readonly string _feedUrl;

    public UpdateService()
        => _feedUrl = OperatingSystem.IsWindows() ? "https://meusttr.net/updates/erpmateus/win" : "https://meusttr.net/updates/erpmateus/linux";
    
    public async Task CheckAndUpdateAsync(UpdateProgressDto dto, CancellationToken ct = default)
    {
        dto.IsUpdating = true;
        dto.HasError = false;
        dto.HasUpdate = false;
        dto.Percent = 0;
        dto.Details = _feedUrl;

        try
        {
            dto.Status = "Verificando atualizações...";
            var mgr = new UpdateManager(_feedUrl);

            // Verifica se há novas versões
            var info = await mgr.CheckForUpdatesAsync();
        
            if (info is null)
            {
                dto.Status = "Você já está na última versão.";
                dto.Percent = 100;
                return;
            }

            dto.HasUpdate = true;
            dto.Status = $"Baixando versão {info.TargetFullRelease.Version}...";

            // Download com reporte de progresso em tempo real
            // O Velopack envia valores de 0 a 100 no parâmetro 'progress'
            await mgr.DownloadUpdatesAsync(info, progress => 
            {
                // Usamos o Dispatcher do Avalonia para garantir que a UI 
                // seja atualizada na Thread principal sem travar
                Avalonia.Threading.Dispatcher.UIThread.Post(() => 
                {
                    dto.Percent = progress;
                });
            }, ct);

            dto.Status = "Download concluído! Reiniciando...";
            dto.Percent = 100;
        
            // Pequena pausa para o usuário ler a mensagem antes do app fechar
            await Task.Delay(1500, ct);

            // Aplica e Reinicia. No Linux, isso faz o swap dos arquivos.
            mgr.ApplyUpdatesAndRestart(info);
        }
        catch (OperationCanceledException)
        {
            dto.Status = "Atualização cancelada.";
            dto.Percent = 0;
        }
        catch (Exception ex)
        {
            dto.HasError = true;
            dto.Status = "Erro na atualização!";
            dto.Details = ex.Message;
            // Log para debug no console do Linux
            Console.WriteLine($"[Velopack Error] {ex}");
        }
        finally
        {
            dto.IsUpdating = false;
        }
    }
    
    // public async Task CheckAndUpdateAsync(UpdateProgressDto dto, CancellationToken ct = default)
    // {
    //     dto.IsUpdating = true;
    //     dto.HasError = false;
    //     dto.HasUpdate = false;
    //     dto.Percent = null;
    //     dto.Details = _feedUrl;
    //
    //     try
    //     {
    //         dto.Status = "Iniciando o ::SISTEMA::";
    //         await Task.Delay(400, ct);
    //         dto.Status = "Verificando atualizações ...";
    //         await Task.Delay(600, ct);
    //         var mgr = new UpdateManager(_feedUrl);
    //         
    //
    //         var info = await mgr.CheckForUpdatesAsync().WaitAsync(ct);
    //         if (info is null)
    //         {
    //             dto.Status = "Você já está na última versão.";
    //             await Task.Delay(200, ct);
    //             dto.Percent = 100;
    //             return;
    //         }
    //
    //         dto.HasUpdate = true;
    //         dto.Status = "Atualização encontrada. Baixando...";
    //         dto.Percent = null;
    //
    //         await mgr.DownloadUpdatesAsync(info).WaitAsync(ct);
    //
    //         dto.Status = "Aplicando atualização e reiniciando...";
    //         await Task.Delay(1000, ct);
    //         dto.Percent = null;
    //
    //         mgr.ApplyUpdatesAndRestart(info);
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         dto.Status = "Atualização cancelada.";
    //     }
    //     catch (Exception ex)
    //     {
    //         dto.HasError = true;
    //         dto.Status = "Erro na atualização!";
    //         dto.Details = ex.Message;
    //     }
    //     finally
    //     {
    //         dto.IsUpdating = false;
    //     }
    // }

    public async Task<(bool UpdatedOrRestarting, string Status)> TryUpdateAsync(CancellationToken ct = default)
    {
        try
        {
            var mgr = new UpdateManager(_feedUrl);

            var info = await mgr.CheckForUpdatesAsync();
            if (info is null)
            {
                Console.WriteLine("Sem atualização!");
                return (false, "Sem atualização.");
            }

            await mgr.DownloadUpdatesAsync(info);

            // Vai reiniciar o app se aplicar.
            mgr.ApplyUpdatesAndRestart(info);
            return (true, "Atualizando e reiniciando...");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return (false, $"Update indisponível: {ex.Message}");
        }
    }
}