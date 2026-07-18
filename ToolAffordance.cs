namespace Cdp.Core;

/// <summary>Tool metadata for catalog = f(phase, object); intent ranks.</summary>
public sealed record ToolAffordance(
    string PrefixedName,
    string Domain,
    string UnderlyingName,
    IReadOnlyList<CdpPhase> Phases,
    IReadOnlyList<CdpObjectKind> Objects,
    IReadOnlyList<CdpIntent> Intents,
    int Cost = 1,
    int Risk = 1,
    string? Hint = null);

public sealed record CatalogHit(ToolAffordance Affordance, int Score);

public sealed class SessionContext
{
    public CdpPhase Phase { get; set; } = CdpPhase.Explore;
    public CdpObjectKind Object { get; set; } = CdpObjectKind.Kb;
    public CdpIntent? Intent { get; set; }

    public string ToJson() =>
        System.Text.Json.JsonSerializer.Serialize(new
        {
            phase = CdpEnumParse.ToWire(Phase),
            @object = CdpEnumParse.ToWire(Object),
            intent = Intent is null ? null : CdpEnumParse.ToWire(Intent.Value)
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
}
