using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;

namespace ERPMateus.ViewModels;

public partial class ConnectionsViewModel : ViewModelBase
{
    private readonly ProfileStore _store = new();
    private readonly SecretsProtector _crypto = new();
    private readonly SqlConnectionTester _tester = new();
    private readonly Action<string> _status;

    public ObservableCollection<DbConnectionProfile> Profiles { get; } = new();

    [ObservableProperty] private DbConnectionProfile? selected;

    // Campos de edição (separados para não “vazar” senha descriptografada na lista)
    [ObservableProperty] private string name = "";
    [ObservableProperty] private string server = "";
    [ObservableProperty] private string database = "";
    [ObservableProperty] private string user = "";
    [ObservableProperty] private string password = "";

    public ConnectionsViewModel(Action<string> statusSink)
    {
        _status = statusSink;
        _ = LoadAsync();
    }

    partial void OnSelectedChanged(DbConnectionProfile? value)
    {
        if (value is null)
        {
            Name = Server = Database = User = Password = "";
            return;
        }

        Name = value.Name;
        Server = value.Server;
        Database = value.Database;
        User = value.User;
        Password = _crypto.DecryptFromBase64(value.EncryptedPassword);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        _status("Carregando conexões...");
        Profiles.Clear();

        var items = await _store.LoadAsync();
        foreach (var p in items.OrderByDescending(x => x.UpdatedAt))
            Profiles.Add(p);

        Selected ??= Profiles.FirstOrDefault();
        _status("Conexões carregadas.");
    }

    [RelayCommand]
    private void New()
    {
        var p = new DbConnectionProfile
        {
            Name = "Nova conexão",
            Server = "",
            Database = "",
            User = "",
            EncryptedPassword = ""
        };

        Profiles.Insert(0, p);
        Selected = p;
        _status("Nova conexão criada (não salva).");
    }

    [RelayCommand]
    private void Delete()
    {
        if (Selected is null) return;

        var toRemove = Selected;
        Profiles.Remove(toRemove);
        Selected = Profiles.FirstOrDefault();

        _status("Removida (falta salvar para persistir).");
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Selected is null)
        {
            _status("Selecione uma conexão para salvar.");
            return;
        }

        Selected.Name = Name.Trim();
        Selected.Server = Server.Trim();
        Selected.Database = Database.Trim();
        Selected.User = User.Trim();
        Selected.EncryptedPassword = _crypto.EncryptToBase64(Password);
        Selected.UpdatedAt = DateTimeOffset.UtcNow;

        await _store.SaveAsync(Profiles.ToList());
        _status("Salvo.");
    }

    [RelayCommand]
    private async Task TestAsync()
    {
        _status("Testando conexão...");
        var res = await _tester.TestAsync(Server.Trim(), Database.Trim(), User.Trim(), Password);
        _status(res.Message);
    }
}