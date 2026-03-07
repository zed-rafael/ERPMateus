using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ERPMateus.Messages;
using ERPMateus.Models;
using ERPMateus.Services;
using ERPMateus.Views.Auxiliar;
using Velopack;

namespace ERPMateus.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IRecipient<SairMessage>
{
    [ObservableProperty] private ViewModelBase? _currentView;
    
    public MainWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<SairMessage>(this);
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

    public void Receive(SairMessage message)
    {
        CurrentView = new LoginViewModel(this);
    }
}