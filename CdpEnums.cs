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

public static class CdpEnumParse
{
    public static bool TryParsePhase(string? raw, out CdpPhase phase) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out phase);

    public static bool TryParseObject(string? raw, out CdpObjectKind obj) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out obj);

    public static bool TryParseIntent(string? raw, out CdpIntent intent) =>
        Enum.TryParse(Normalize(raw), ignoreCase: true, out intent);

    public static string ToWire(CdpPhase p) => p.ToString().ToLowerInvariant();
    public static string ToWire(CdpObjectKind o) => o.ToString().ToLowerInvariant();
    public static string ToWire(CdpIntent i) => i.ToString().ToLowerInvariant();

    private static string Normalize(string? raw) =>
        string.IsNullOrWhiteSpace(raw) ? "" : raw.Trim();
}
