using System;

namespace ERPMateus.ViewModels;

public class DbConnectionProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "Nova conexão";

    public string Server { get; set; } = "";
    public string Database { get; set; } = "";

    public string User { get; set; } = "";

    // Armazenaremos a senha criptografada (base64).
    public string EncryptedPassword { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}