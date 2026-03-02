using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ERPMateus.ViewModels;

namespace ERPMateus.Services;

public sealed class ProfileStore
{
    private readonly string _filePath;

    private static readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true
    };

    public ProfileStore(string appName = "SigaCredenciais")
    {
        var baseDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            appName);

        Directory.CreateDirectory(baseDir);
        _filePath = Path.Combine(baseDir, "profiles.json");
    }

    public async Task<List<DbConnectionProfile>> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath))
            return new List<DbConnectionProfile>();

        var json = await File.ReadAllTextAsync(_filePath, ct);
        return JsonSerializer.Deserialize<List<DbConnectionProfile>>(json, _json) ?? new();
    }

    public async Task SaveAsync(List<DbConnectionProfile> profiles, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(profiles, _json);
        await File.WriteAllTextAsync(_filePath, json, ct);
    }
}