using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Models;

namespace ERPMateus.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [NotifyCanExecuteChangedFor(nameof(ExecutarLoginCommand))]
    [ObservableProperty] private string _username;
    [NotifyCanExecuteChangedFor(nameof(ExecutarLoginCommand))]
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _errorMessage;
    [ObservableProperty] private bool _loginSucesso;
    [ObservableProperty] private bool _habilitarEntrar;
    private readonly Action<Usuario> _onLoginSucesso;

    public LoginViewModel(Action<Usuario> onLoginSucesso)
    {
        _onLoginSucesso = onLoginSucesso;
    }
    
    [RelayCommand(CanExecute = nameof(PodeExecutarLogin))]
    private async Task ExecutarLogin()
    {
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