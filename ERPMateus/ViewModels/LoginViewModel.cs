using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERPMateus.Database;
using ERPMateus.Models;
using ERPMateus.Services;
using ERPMateus.ViewModels.Dto;

namespace ERPMateus.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindow;
    
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _statusProcesso = "Processando...";
    [ObservableProperty] private string _errorMessage = string.Empty;
    
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
    private async Task ExecutarLogin()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;
        
        ErrorMessage = string.Empty;

        try
        {
            IsLoading = true;
            StatusProcesso = "Verificando usuário...";
            await Task.Delay(400);
            Usuario user = await new DbUsuario().DBSelect(Username, Password);
            
            StatusProcesso = "Obtendo itens do menu...";
            await Task.Delay(600);
            //user.MenuItems = await new DBSigaMenuItem().CarregarMenuItensAsync();
            
            StatusProcesso = "Configurando o sistema...";
            await Task.Delay(300);
            
            StatusProcesso = "Aplicando o tema de cores...";
            await Task.Delay(300);
            
            if (user is { UsuarioID: > 0 })
            {
                Global.InfoAplicacao.Usuario = user;
                //TemaService.AplicarTemaCores(user.UsuarioCores); 
                _mainWindow.ShowMainApp();
            }
            else
            {
                ErrorMessage = "Usuário ou senha inválidos.";
            }
        }
        catch (Exception e)
        {
            ErrorMessage = $"Erro de conexão com o banco!";
            IsLoading = false;
        }
        finally
        {
            IsLoading = false;
        }
    }
}