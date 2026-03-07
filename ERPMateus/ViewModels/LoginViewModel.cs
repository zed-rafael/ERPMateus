using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Models;
using ERPMateus.Services;
using ERPMateus.ViewModels.Dto;

namespace ERPMateus.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindow;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Preenchimento obrigatório")] 
    [MaxLength(50, ErrorMessage = "Máximo de 50 caracteres")] 
    private string? _username = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Preenchimento obrigatório")] 
    [MinLength(4, ErrorMessage = "Mínimo de 4 caracteres")] 
    [MaxLength(40, ErrorMessage = "Máximo de 40 caracteres")] 
    private string? _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PasswordChar))]
    private bool _isPasswordVisible;

    public char PasswordChar => IsPasswordVisible ? '\0' : '*';
    
    private readonly UpdateService _updates;
    public UpdateProgressDto UpdateDto { get; } = new();

    public LoginViewModel(MainWindowViewModel mainWindow)
    {
        _mainWindow = mainWindow;
        _updates = new UpdateService();
        
        _ = CheckUpdatesOnLoginAsync();
    }
    
    private async Task CheckUpdatesOnLoginAsync()
    {
        await _updates.CheckAndUpdateAsync(UpdateDto);
    }

    [RelayCommand]
    private void ExecutarLogin()
    {
        ValidateAllProperties();
        
        if (HasErrors)
        {
            return;
        }
        _mainWindow.ShowMainApp();
    }
}