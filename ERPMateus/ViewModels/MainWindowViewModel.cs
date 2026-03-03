using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Services;
using ERPMateus.Views.Auxiliar;
using Velopack;

namespace ERPMateus.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public LoginViewModel LoginViewModel { get; } = new();
    public DashboardViewModel DashboardViewModel { get; } = new();

    [ObservableProperty] private int? paginaAplicacao = 0;
}