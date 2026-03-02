using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ERPMateus.Services;

public class SecretsProtector
{
    private readonly string _keyPath;

    public SecretsProtector(string appName = "SigaCredenciais")
    {
        var baseDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            appName);

        Directory.CreateDirectory(baseDir);
        _keyPath = Path.Combine(baseDir, "secret.key");
    }

    public string EncryptToBase64(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return "";

        var key = LoadOrCreateKey();

        var nonce = RandomNumberGenerator.GetBytes(12);
        var plaintextBytes = Encoding.UTF8.GetBytes(plainText);

        var cipher = new byte[plaintextBytes.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(key);
        aes.Encrypt(nonce, plaintextBytes, cipher, tag);

        // payload = nonce(12) + tag(16) + cipher(n)
        var payload = new byte[12 + 16 + cipher.Length];
        Buffer.BlockCopy(nonce, 0, payload, 0, 12);
        Buffer.BlockCopy(tag, 0, payload, 12, 16);
        Buffer.BlockCopy(cipher, 0, payload, 28, cipher.Length);

        return Convert.ToBase64String(payload);
    }

    public string DecryptFromBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64)) return "";

        var payload = Convert.FromBase64String(base64);
        if (payload.Length < 12 + 16) return "";

        var key = LoadOrCreateKey();

        var nonce = payload[..12];
        var tag = payload[12..28];
        var cipher = payload[28..];

        var plaintext = new byte[cipher.Length];

        using var aes = new AesGcm(key);
        aes.Decrypt(nonce, cipher, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    private byte[] LoadOrCreateKey()
    {
        if (File.Exists(_keyPath))
        {
            var protectedKey = File.ReadAllBytes(_keyPath);
            return UnprotectKey(protectedKey);
        }

        var key = RandomNumberGenerator.GetBytes(32);
        var protectedBytes = ProtectKey(key);

        File.WriteAllBytes(_keyPath, protectedBytes);

        // Baseline: tenta restringir acesso no Linux/macOS
        TryRestrictFilePermissions(_keyPath);

        return key;
    }

    private static byte[] ProtectKey(byte[] key)
    {
#if WINDOWS
        return ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);
#else
        // Sem DPAPI: armazena “como está” e depende de ACL do arquivo.
        return key;
#endif
    }

    private static byte[] UnprotectKey(byte[] protectedKey)
    {
#if WINDOWS
        return ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);
#else
        return protectedKey;
#endif
    }

    private static void TryRestrictFilePermissions(string path)
    {
        try
        {
            // Em Windows isso é irrelevante; em Linux/macOS ajuda.
            // 600: somente usuário.
            if (!OperatingSystem.IsWindows())
            {
                // melhor esforço; pode falhar sem chmod disponível
                var chmod = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "chmod",
                    ArgumentList = { "600", path },
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                System.Diagnostics.Process.Start(chmod)?.WaitForExit();
            }
        }
        catch { /* best-effort */ }
    }
}