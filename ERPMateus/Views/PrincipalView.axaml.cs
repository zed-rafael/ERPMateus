using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ERPMateus.ViewModels;

namespace ERPMateus.Views;

public partial class PrincipalView : UserControl
{
    public PrincipalView()
    {
        InitializeComponent();
        DataContext = new PrincipalViewModel();
    }
}