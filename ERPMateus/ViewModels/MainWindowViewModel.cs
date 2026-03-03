using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Models;
using ERPMateus.Services;
using ERPMateus.Views.Auxiliar;
using Velopack;

namespace ERPMateus.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const int PAGINA_LOGIN = 0;
    private const int PAGINA_PAINEL = 1;
    private const string MSG_BEM_VINDO = "Faça login para entrar";
    [ObservableProperty] private Usuario? _usuario;
    [ObservableProperty] private bool _estaLogado;
    [ObservableProperty] private string? _bemVindo = MSG_BEM_VINDO;
    
    public LoginViewModel LoginViewModel { get; }
    public PrincipalViewModel PrincipalViewModel { get; } = new();
    public DashboardViewModel DashboardViewModel { get; } = new();

    [ObservableProperty] private int? paginaAplicacao = 0;

    public MainWindowViewModel()
    {
        Usuario = new Usuario();
        LoginViewModel = new LoginViewModel(OnLoginSucesso);
        EstaLogado = false;
        PaginaAplicacao = PAGINA_LOGIN;
    }
    
    private void OnLoginSucesso(Usuario usuario)
    {
        //__GLOBAL._USUARIO = usuario;
        Console.WriteLine("Metodo dentro de main sucesso!");
        Usuario = usuario;
        EstaLogado = true;
        BemVindo = usuario.UsuarioNome;
        PaginaAplicacao = PAGINA_PAINEL;
    }
}