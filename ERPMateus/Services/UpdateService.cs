using System;
using System.Threading;
using System.Threading.Tasks;
using ERPMateus.ViewModels.Dto;
using Velopack;

namespace ERPMateus.Services;

public sealed class UpdateService
{
    private readonly string _feedUrl;

    public UpdateService(string feedUrl)
        => _feedUrl = feedUrl;
    
    public async Task CheckAndUpdateAsync(UpdateProgressDto dto, CancellationToken ct = default)
    {
        dto.IsUpdating = true;
        dto.HasError = false;
        dto.HasUpdate = false;
        dto.Percent = null;
        dto.Details = _feedUrl;

        try
        {
            //https://meusttr.net/updates/erpmateus/
            dto.Status = "Verificando atualizações 2...";
            await Task.Delay(3000, ct);
            var mgr = new UpdateManager(_feedUrl);
            

            var info = await mgr.CheckForUpdatesAsync().WaitAsync(ct);
            if (info is null)
            {
                dto.Status = "Você já está na última versão.";
                await Task.Delay(2000, ct);
                dto.Percent = 100;
                return;
            }

            dto.HasUpdate = true;
            dto.Status = "Atualização encontrada. Baixando...";
            dto.Percent = null;

            await mgr.DownloadUpdatesAsync(info).WaitAsync(ct);

            dto.Status = "Aplicando atualização e reiniciando...";
            await Task.Delay(1000, ct);
            dto.Percent = null;

            mgr.ApplyUpdatesAndRestart(info);

            Environment.Exit(0);
        }
        catch (OperationCanceledException)
        {
            dto.Status = "Atualização cancelada.";
        }
        catch (Exception ex)
        {
            dto.HasError = true;
            dto.Status = "Erro na atualização!";
            dto.Details = ex.Message;
        }
        finally
        {
            dto.IsUpdating = false;
        }
    }

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