using CommunityToolkit.Mvvm.ComponentModel;

namespace ERPMateus.ViewModels;

public partial class UpdateDialogViewModel : ViewModelBase
{
    [ObservableProperty] private string title = "Atualização";
    [ObservableProperty] private string message = "Preparando...";
    [ObservableProperty] private bool isBusy = true;

    // 0..100 (use null para indeterminado)
    [ObservableProperty] private double? progress;

    [ObservableProperty] private string? details;
    [ObservableProperty] private bool hasError;
    [ObservableProperty] private string? errorMessage;

    public void SetStep(string msg, double? pct = null, string? details = null)
    {
        Message = msg;
        Progress = pct;
        Details = details;
    }

    public void SetError(string msg, string? details = null)
    {
        HasError = true;
        ErrorMessage = msg;
        Details = details;
        IsBusy = false;
        Progress = null;
    }

    public void SetDone(string msg = "Atualização concluída.")
    {
        Message = msg;
        IsBusy = false;
        Progress = 100;
    }
}