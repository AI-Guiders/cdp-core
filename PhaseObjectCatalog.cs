namespace Cdp.Core;

public static class PhaseObjectCatalog
{
    public static IReadOnlyList<CatalogHit> Query(
        IEnumerable<ToolAffordance> all,
        CdpPhase phase,
        CdpObjectKind obj,
        CdpIntent? intent = null,
        int limit = 40)
    {
        var lim = Math.Clamp(limit, 1, 200);
        var hits = new List<CatalogHit>();
        foreach (var a in all)
        {
            if (!a.Phases.Contains(phase))
                continue;
            if (!a.Objects.Contains(obj))
                continue;
            var score = 100 - a.Cost * 3 - a.Risk * 5;
            if (intent is { } want)
            {
                if (a.Intents.Count > 0 && !a.Intents.Contains(want))
                    score -= 40;
                else if (a.Intents.Contains(want))
                    score += 25;
            }
            hits.Add(new CatalogHit(a, score));
        }

        return hits
            .OrderByDescending(h => h.Score)
            .ThenBy(h => h.Affordance.PrefixedName, StringComparer.Ordinal)
            .Take(lim)
            .ToList();
    }

    public static IReadOnlyList<ToolAffordance> DefaultColdStart(IEnumerable<ToolAffordance> all) =>
        Query(all, CdpPhase.Explore, CdpObjectKind.Kb, CdpIntent.Find, limit: 12)
            .Select(h => h.Affordance)
            .ToList();
}
