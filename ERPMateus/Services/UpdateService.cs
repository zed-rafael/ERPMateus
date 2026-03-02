using System;
using System.Threading;
using System.Threading.Tasks;
using Velopack;

namespace ERPMateus.Services;

public sealed class UpdateService
{
    private readonly string _feedUrl;

    public UpdateService(string feedUrl)
    {
        _feedUrl = feedUrl;
    }

    public async Task<(bool UpdatedOrRestarting, string Status)> TryUpdateAsync(CancellationToken ct = default)
    {
        try
        {
            var mgr = new UpdateManager(_feedUrl);

            var info = await mgr.CheckForUpdatesAsync();
            if (info is null)
                return (false, "Sem atualização.");

            await mgr.DownloadUpdatesAsync(info);

            // Vai reiniciar o app se aplicar.
            mgr.ApplyUpdatesAndRestart(info);
            return (true, "Atualizando e reiniciando...");
        }
        catch (Exception ex)
        {
            // Em dev é comum falhar (não empacotado).
            return (false, $"Update indisponível: {ex.Message}");
        }
    }
}