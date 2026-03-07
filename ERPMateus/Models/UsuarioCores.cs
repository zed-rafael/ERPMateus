namespace ERPMateus.Models;

public class UsuarioCores
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public string Accent { get; set; } 
    public string CorAtiva { get; set; } 
    public string CorPassagem { get; set; } 
    public string CorPrincipal { get; set; }
    public string CorSecundaria { get; set; }
    public string CorBordaPrincipal { get; set; }
    public string CorBordaPrincipalControles { get; set; }
}