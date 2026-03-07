using System;

namespace ERPMateus.Models;

public class Usuario
{
    //Nivel 21 - cemar
    //Nivel 22 - joao.financas (bloco seriado, boletos e negociacoes)

    public Usuario()
    {
        UsuarioCores = new UsuarioCores();
    }
    
    public int UsuarioID { get; set; }
    public string UsuarioNome { get; set; } = "*Usuario";
    public string UsuarioEmail { get; set; }
    public string UsuarioSenha { get; set; }
    public string UsuarioSetor { get; set; }
    public int UsuarioNivel { get; set; }
    public DateTime? UsuarioDtAcesso { get; set; }
    public int UsuarioSindicato { get; set; }
    public string UsuarioUF { get; set; }
    public int UsuarioSecretaria { get; set; }
    public int RegionalId { get; set; }
    public int ConfiguracaoAparenciaId { get; set; }

    public UsuarioCores UsuarioCores { get; set; }
    public DateTime? DATE_SERVER { get; set; }

    public string NomeCompleto { get; set; }
    public string Cpf { get; set; }
    
}