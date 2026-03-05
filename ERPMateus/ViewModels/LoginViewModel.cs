using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Models;
using ERPMateus.Services;
using ERPMateus.ViewModels.Dto;

namespace ERPMateus.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _errorMessage;
    [ObservableProperty] private bool _loginSucesso;
    [ObservableProperty] private bool _habilitarEntrar;
    private readonly Action<Usuario> _onLoginSucesso;
    
    private readonly UpdateService _updates;

    public UpdateProgressDto UpdateDto { get; } = new();

    public LoginViewModel(Action<Usuario> onLoginSucesso)
    {
        _onLoginSucesso = onLoginSucesso;
        _updates = new UpdateService();
        
        _ = CheckUpdatesOnLoginAsync();
    }
    
    private async Task CheckUpdatesOnLoginAsync()
    {
        await _updates.CheckAndUpdateAsync(UpdateDto);
    }

    // Opção 2: um botão "Verificar"
    [RelayCommand]
    private async Task CheckUpdates()
    {
        await _updates.CheckAndUpdateAsync(UpdateDto);
    }

    [RelayCommand]
    private void Testar()
    {
        Console.WriteLine("ExecutarLogin");
        // Usuario usuario = await new DBUsuario().DBSelect(Username, Password);
        Usuario usuario = new Usuario() { UsuarioID = 10, NomeCompleto = "Zeuxis Rafael"};
        if (usuario == null)
        {
            LoginSucesso = false;
            ErrorMessage = "Usuário ou senha inválido.";
            Console.WriteLine("Usuario null");
            return;
        }
        _onLoginSucesso?.Invoke(usuario);
        LoginSucesso = true;
        Console.WriteLine("LoginSucesso!!");
    }

    [RelayCommand]
    private void ExecutarLogin()
    {
        Console.WriteLine("ExecutarLogin");
        // Usuario usuario = await new DBUsuario().DBSelect(Username, Password);
        Usuario usuario = new Usuario() { UsuarioID = 10, NomeCompleto = "Zeuxis Rafael"};
        if (usuario == null)
        {
            LoginSucesso = false;
            ErrorMessage = "Usuário ou senha inválido.";
            Console.WriteLine("Usuario null");
            return;
        }
        _onLoginSucesso?.Invoke(usuario);
        LoginSucesso = true;
        Console.WriteLine("LoginSucesso!!");
    }
    
    private bool PodeExecutarLogin()
    {
        return true;
        // return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
    }
}