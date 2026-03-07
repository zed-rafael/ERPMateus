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
    [ObservableProperty] private ViewModelBase? _currentView;
    
    public MainWindowViewModel()
    {
        Global.InfoAplicacao.Usuario = new Usuario()
        {
            UsuarioNome = "*Usuário"
        };
        CurrentView = new LoginViewModel(this);
    }

    public void ShowMainApp()
    {
        CurrentView = new PrincipalViewModel();
    }
}