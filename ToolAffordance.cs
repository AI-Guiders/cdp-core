namespace Cdp.Core;

/// <summary>
/// Tool metadata for catalog = f(phase, object [, language]); intent ranks.
/// Language sits beside phase×object — not a free-text goal.
/// </summary>
public sealed record ToolAffordance(
    string PrefixedName,
    string Domain,
    string UnderlyingName,
    IReadOnlyList<CdpPhase> Phases,
    IReadOnlyList<CdpObjectKind> Objects,
    IReadOnlyList<CdpIntent> Intents,
    int Cost = 1,
    int Risk = 1,
    string? Hint = null,
    IReadOnlyList<CdpLanguage>? Languages = null)
{
    /// <summary>Empty/null Languages → <see cref="CdpLanguage.Any"/> (agnostic).</summary>
    public IReadOnlyList<CdpLanguage> EffectiveLanguages =>
        Languages is { Count: > 0 } ? Languages : [CdpLanguage.Any];
}

public sealed record CatalogHit(ToolAffordance Affordance, int Score);

public sealed class SessionContext
{
    public CdpPhase Phase { get; set; } = CdpPhase.Explore;
    public CdpObjectKind Object { get; set; } = CdpObjectKind.Kb;
    public CdpIntent? Intent { get; set; }
    /// <summary>Null or <see cref="CdpLanguage.Any"/> = no language filter.</summary>
    public CdpLanguage? Language { get; set; }

    public string ToJson() =>
        System.Text.Json.JsonSerializer.Serialize(new
        {
            phase = CdpEnumParse.ToWire(Phase),
            @object = CdpEnumParse.ToWire(Object),
            intent = Intent is null ? null : CdpEnumParse.ToWire(Intent.Value),
            language = Language is null ? null : CdpEnumParse.ToWire(Language.Value)
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
}
