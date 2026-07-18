using Cdp.Core;
using Xunit;

namespace Cdp.Core.Tests;

public class PhaseObjectCatalogTests
{
    private static readonly IReadOnlyList<ToolAffordance> Seed = Wave1AffordanceSeed.Build();

    [Fact]
    public void Explore_Kb_Includes_KnowledgeTags()
    {
        var hits = PhaseObjectCatalog.Query(Seed, CdpPhase.Explore, CdpObjectKind.Kb, CdpIntent.Cite);
        Assert.Contains(hits, h => h.Affordance.PrefixedName == "an_knowledge_tags");
        Assert.DoesNotContain(hits, h => h.Affordance.PrefixedName == "an_write_knowledge_file");
    }

    [Fact]
    public void Act_Task_Includes_TaskUpsert()
    {
        var hits = PhaseObjectCatalog.Query(Seed, CdpPhase.Act, CdpObjectKind.Task, CdpIntent.Change);
        Assert.Contains(hits, h => h.Affordance.PrefixedName == "tk_task_upsert");
    }

    [Fact]
    public void Intent_Cite_Ranks_Tags_Above_ListFiles()
    {
        var hits = PhaseObjectCatalog.Query(Seed, CdpPhase.Explore, CdpObjectKind.Kb, CdpIntent.Cite, limit: 20);
        var tags = hits.First(h => h.Affordance.PrefixedName == "an_knowledge_tags");
        var list = hits.FirstOrDefault(h => h.Affordance.PrefixedName == "an_list_knowledge_files");
        if (list is not null)
            Assert.True(tags.Score >= list.Score);
    }

    [Fact]
    public void ColdStart_Is_Small()
    {
        var cold = PhaseObjectCatalog.DefaultColdStart(Seed);
        Assert.InRange(cold.Count, 1, 12);
        Assert.All(cold, a => Assert.Contains(CdpObjectKind.Kb, a.Objects));
    }

    [Fact]
    public void Language_Python_Hides_Csharp_Only_Debug()
    {
        var hits = PhaseObjectCatalog.Query(
            Seed,
            CdpPhase.Act,
            CdpObjectKind.Code,
            CdpIntent.Change,
            limit: 40,
            language: CdpLanguage.Python);
        Assert.DoesNotContain(hits, h => h.Affordance.PrefixedName == "dbg_debug_launch");
        Assert.DoesNotContain(hits, h => h.Affordance.Domain == "dbg");
    }

    [Fact]
    public void Language_Csharp_Keeps_Debug_And_Boosts()
    {
        var withCs = PhaseObjectCatalog.Query(
            Seed, CdpPhase.Act, CdpObjectKind.Process, CdpIntent.Change, limit: 20, language: CdpLanguage.Csharp);
        var launch = Assert.Single(withCs, h => h.Affordance.PrefixedName == "dbg_debug_launch");
        var noLang = PhaseObjectCatalog.Query(
            Seed, CdpPhase.Act, CdpObjectKind.Process, CdpIntent.Change, limit: 20);
        var launchAny = Assert.Single(noLang, h => h.Affordance.PrefixedName == "dbg_debug_launch");
        Assert.True(launch.Score > launchAny.Score);
    }
}
