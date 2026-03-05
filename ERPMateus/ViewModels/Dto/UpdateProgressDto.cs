using CommunityToolkit.Mvvm.ComponentModel;

namespace ERPMateus.ViewModels.Dto;

public partial class UpdateProgressDto : ViewModelBase
{
    [ObservableProperty] private bool isUpdating;
    [ObservableProperty] private string status;
    [ObservableProperty] private double? percent; // null = indeterminado
    [ObservableProperty] private string? details;
    [ObservableProperty] private bool hasUpdate;
    [ObservableProperty] private bool hasError;

    public UpdateProgressDto()
    {
        IsUpdating = true;
        Status = "Verificando atualizações...";
        HasError = false;
        HasUpdate = false;
    }
}