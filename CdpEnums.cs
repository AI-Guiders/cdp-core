namespace Cdp.Core;

public enum CdpPhase
{
    Explore,
    Clarify,
    Act,
    Verify,
    Handoff
}

public enum CdpObjectKind
{
    Kb,
    Code,
    Repo,
    Task,
    Finding,
    Process,
    Issue,
    Session
}

public enum CdpIntent
{
    Find,
    Cite,
    Change,
    Verify,
    Record,
    Ship
}

/// <summary>
/// Stack/language axis beside phase×object (not a goal).
/// <see cref="Any"/> = language-agnostic affordance (KB, TK, session, …).
/// </summary>
public enum CdpLanguage
{
    Any,
    Csharp,
    Python,
    Delphi
}

public static class CdpEnumParse
{
    public static bool TryParsePhase(string? raw, out CdpPhase phase) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out phase);

    public static bool TryParseObject(string? raw, out CdpObjectKind obj) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out obj);

    public static bool TryParseIntent(string? raw, out CdpIntent intent) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out intent);

    public static bool TryParseLanguage(string? raw, out CdpLanguage language)
    {
        language = default;
        if (raw is null)
            return false;
        var n = Normalize(raw);
        if (n.Length == 0)
            return false;
        if (n is "any" or "*")
        {
            language = CdpLanguage.Any;
            return true;
        }

        n = n switch
        {
            "cs" or "c#" or "csharp" => nameof(CdpLanguage.Csharp),
            "py" or "python" => nameof(CdpLanguage.Python),
            "pas" or "delphi" or "objectpascal" => nameof(CdpLanguage.Delphi),
            _ => n
        };
        return Enum.TryParse(n, ignoreCase: true, out language);
    }

    public static string ToWire(CdpPhase p) => p.ToString().ToLowerInvariant();
    public static string ToWire(CdpObjectKind o) => o.ToString().ToLowerInvariant();
    public static string ToWire(CdpIntent i) => i.ToString().ToLowerInvariant();
    public static string ToWire(CdpLanguage l) => l.ToString().ToLowerInvariant();

    private static string Normalize(string? raw) =>
        string.IsNullOrWhiteSpace(raw) ? "" : raw.Trim();
}
