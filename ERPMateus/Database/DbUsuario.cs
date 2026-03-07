using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using ERPMateus.Models;
using Microsoft.Data.SqlClient;

namespace ERPMateus.Database;

public class DbUsuario
{
    public async Task<Usuario> DBSelect(string email, string senha)
    {
        Usuario? obj = null;
        var sql = new StringBuilder();
        sql.Append(" SET NOCOUNT ON; ");
        sql.Append(" SELECT u.id, u.nome, u.email, u.senha, u.setor, u.nivel, u.dt_acesso, u.secretaria, u.sindicato, u.UF, GETDATE(), u.NomeCompleto, u.Cpf, u.Regional, ");
        sql.Append(" u.ConfiguracaoAparenciaId, t.Accent, t.CorAtiva, t.CorPassagem, t.CorPrincipal, t.CorSecundaria, t.PrincipalCorBorda, t.PrincipalCorBordaControles ");
        sql.Append(" FROM fetaema_siga_uol.dbo.usuario u LEFT JOIN fetaema_siga_uol.dbo.UsuarioTemaCores t ON u.ConfiguracaoAparenciaId = t.Id ");
        sql.Append(" WHERE u.email=@email AND u.senha=@senha ");
        
        using (var connection = new SqlConnection(Global.ConnectionString))
        using (var command = new SqlCommand(sql.ToString(), connection))
        {
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@senha", senha);

            await connection.OpenAsync();

            using (var rd = await command.ExecuteReaderAsync())
            {
                if (await rd.ReadAsync())
                {
                    obj = new Usuario();
                    if (!rd.IsDBNull(0)) obj.UsuarioID = Convert.ToInt32(rd[0]);
                    if (!rd.IsDBNull(1)) obj.UsuarioNome = Convert.ToString(rd[1]);
                    if (!rd.IsDBNull(2)) obj.UsuarioEmail = Convert.ToString(rd[2]);
                    if (!rd.IsDBNull(3)) obj.UsuarioSenha = Convert.ToString(rd[3]);
                    if (!rd.IsDBNull(4)) obj.UsuarioSetor = Convert.ToString(rd[4]);
                    if (!rd.IsDBNull(5)) obj.UsuarioNivel = Convert.ToInt32(rd[5]);
                    if (!rd.IsDBNull(6)) obj.UsuarioDtAcesso = Convert.ToDateTime(rd[6]);
                    if (!rd.IsDBNull(7)) obj.UsuarioSecretaria = Convert.ToInt32(rd[7]);
                    if (!rd.IsDBNull(8)) obj.UsuarioSindicato = Convert.ToInt32(rd[8]);
                    if (!rd.IsDBNull(9)) obj.UsuarioUF = Convert.ToString(rd[9]);
                    if (!rd.IsDBNull(10)) obj.DATE_SERVER = Convert.ToDateTime(rd[10]);
                    if (!rd.IsDBNull(11)) obj.NomeCompleto = Convert.ToString(rd[11]);
                    if (!rd.IsDBNull(12)) obj.Cpf = Convert.ToString(rd[12]);
                    if (!rd.IsDBNull(13)) obj.RegionalId = Convert.ToInt32(rd[13]);
                    
                    if (!rd.IsDBNull(15)) obj.UsuarioCores.Accent = Convert.ToString(rd[15]);
                    if (!rd.IsDBNull(16)) obj.UsuarioCores.CorAtiva = Convert.ToString(rd[16]);
                    if (!rd.IsDBNull(17)) obj.UsuarioCores.CorPassagem = Convert.ToString(rd[17]);
                    if (!rd.IsDBNull(18)) obj.UsuarioCores.CorPrincipal = Convert.ToString(rd[18]);
                    if (!rd.IsDBNull(19)) obj.UsuarioCores.CorSecundaria = Convert.ToString(rd[19]);
                    if (!rd.IsDBNull(20)) obj.UsuarioCores.CorBordaPrincipal = Convert.ToString(rd[20]);
                    if (!rd.IsDBNull(21)) obj.UsuarioCores.CorBordaPrincipalControles = Convert.ToString(rd[21]);
                }
                
            }
        }

        return obj;
    }
}