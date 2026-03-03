using System.Reflection;

namespace ERPMateus.Services;

public static class AppInfo
{
    public static string Version => Clean(GetInformationalVersion());

    private static string GetInformationalVersion()
    {
        var asm = Assembly.GetExecutingAssembly();

        var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (!string.IsNullOrWhiteSpace(info))
            return info;

        return asm.GetName().Version?.ToString() ?? "0.0.0";
    }

    private static string Clean(string v)
    {
        // Ex: "1.0.0+abc123" -> "1.0.0"
        var plus = v.IndexOf('+');
        if (plus >= 0) v = v[..plus];

        // Se você NÃO quer mostrar prerelease, descomente:
        // var dash = v.IndexOf('-');
        // if (dash >= 0) v = v[..dash];

        return v.Trim();
    }
}