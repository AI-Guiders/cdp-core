namespace Cdp.Core;

/// <summary>Seed affordances — Memory.* / debug / build wire names (intuition-aligned).</summary>
public static class Wave1AffordanceSeed
{
    private static readonly CdpPhase[] ExploreClarify = [CdpPhase.Explore, CdpPhase.Clarify];
    private static readonly CdpPhase[] ExploreAct = [CdpPhase.Explore, CdpPhase.Act];
    private static readonly CdpPhase[] ActVerify = [CdpPhase.Act, CdpPhase.Verify];
    private static readonly CdpPhase[] AllPhases =
    [
        CdpPhase.Explore, CdpPhase.Clarify, CdpPhase.Act, CdpPhase.Verify, CdpPhase.Handoff
    ];

    private static readonly CdpLanguage[] AnyLang = [CdpLanguage.Any];
    private static readonly CdpLanguage[] CsharpLang = [CdpLanguage.Csharp];

    public static IReadOnlyList<ToolAffordance> Build()
    {
        var list = new List<ToolAffordance>();

        // Memory.World / Project / Skill — same knowledge underlyings; scope enforced in host.
        foreach (var domain in new[] { CdpDomains.MemoryWorld, CdpDomains.MemoryProject, CdpDomains.MemorySkill })
        {
            list.AddRange(KnowledgeFacet(domain));
        }

        // Memory.Session — hot / notes continuity (not KB tree)
        list.AddRange(
        [
            A(CdpDomains.MemorySession, "route_context", ExploreClarify, [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Find], 2, 1),
            A(CdpDomains.MemorySession, "read_hot_context", ExploreClarify.Concat([CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
            A(CdpDomains.MemorySession, "memory_health", [CdpPhase.Explore, CdpPhase.Verify], [CdpObjectKind.Session], [CdpIntent.Verify], 1, 1),
            A(CdpDomains.MemorySession, "search_agent_notes", ExploreClarify, [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Find], 2, 1),
            A(CdpDomains.MemorySession, "upsert_agent_notes_section", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Session], [CdpIntent.Change, CdpIntent.Record], 2, 3),
            A(CdpDomains.MemorySession, "validate_sections", ActVerify, [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Verify], 1, 1),
            A(CdpDomains.MemorySession, "normalize_sections", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Verify], 2, 2),
        ]);

        // Memory.Task
        list.AddRange(
        [
            A(CdpDomains.MemoryTask, "man", AllPhases, [CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemoryTask, "ensure_store", ExploreAct, [CdpObjectKind.Task], [CdpIntent.Change], 2, 2),
            A(CdpDomains.MemoryTask, "route_next", ExploreClarify.Concat([CdpPhase.Act, CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Task], [CdpIntent.Find, CdpIntent.Ship], 1, 1),
            A(CdpDomains.MemoryTask, "tasks", ExploreClarify, [CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemoryTask, "task_upsert", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Task], [CdpIntent.Change, CdpIntent.Record], 2, 2),
            A(CdpDomains.MemoryTask, "read_card", ExploreClarify.Concat([CdpPhase.Act]).ToArray(), [CdpObjectKind.Task], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
            A(CdpDomains.MemoryTask, "write_card", [CdpPhase.Act], [CdpObjectKind.Task], [CdpIntent.Change], 3, 3),
            A(CdpDomains.MemoryTask, "upsert_section", [CdpPhase.Act], [CdpObjectKind.Task], [CdpIntent.Change, CdpIntent.Record], 2, 2),
            A(CdpDomains.MemoryTask, "analytics_upsert", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Task], [CdpIntent.Record], 2, 2),
        ]);

        // Memory.Self.Finding
        list.AddRange(
        [
            A(CdpDomains.MemorySelfFinding, "man", AllPhases, [CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemorySelfFinding, "findings", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemorySelfFinding, "finding_record", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Record], 2, 2),
            A(CdpDomains.MemorySelfFinding, "finding_check", ActVerify, [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Verify], 1, 1),
            A(CdpDomains.MemorySelfFinding, "tasks", ExploreClarify, [CdpObjectKind.Finding, CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemorySelfFinding, "task_record", [CdpPhase.Act], [CdpObjectKind.Finding, CdpObjectKind.Task], [CdpIntent.Record], 2, 2),
        ]);

        // Memory.Self.Failure
        list.AddRange(
        [
            A(CdpDomains.MemorySelfFailure, "man", AllPhases, [CdpObjectKind.Process, CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemorySelfFailure, "failures", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Process, CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
            A(CdpDomains.MemorySelfFailure, "failure_record", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Process], [CdpIntent.Record], 2, 2),
        ]);

        // Dev.Debug — underlying = DAP catalog names (debug_*)
        list.AddRange(
        [
            A(CdpDomains.Debug, "man", AllPhases, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find], 1, 1, "ops: stop before rebuild", CsharpLang),
            A(CdpDomains.Debug, "debug_ping", ExploreAct.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Process], [CdpIntent.Verify], 1, 1, null, CsharpLang),
            A(CdpDomains.Debug, "debug_set_breakpoints", [CdpPhase.Act], [CdpObjectKind.Code], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_list_breakpoints", ExploreAct.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Code], [CdpIntent.Find], 1, 1, null, CsharpLang),
            A(CdpDomains.Debug, "debug_clear_breakpoints", [CdpPhase.Act], [CdpObjectKind.Code], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_launch", [CdpPhase.Act], [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 3, 4, null, CsharpLang),
            A(CdpDomains.Debug, "debug_attach", [CdpPhase.Act], [CdpObjectKind.Process], [CdpIntent.Change], 3, 4, null, CsharpLang),
            A(CdpDomains.Debug, "debug_continue", ActVerify, [CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_step_over", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_step_into", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_step_out", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
            A(CdpDomains.Debug, "debug_stop", ActVerify.Concat([CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Process], [CdpIntent.Change], 1, 2, "prefer over taskkill", CsharpLang),
            A(CdpDomains.Debug, "debug_stop_context", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, "after stopped", CsharpLang),
            A(CdpDomains.Debug, "debug_stack_trace", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
            A(CdpDomains.Debug, "debug_variables", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
            A(CdpDomains.Debug, "debug_variable_children", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
        ]);

        // Dev.Build
        list.AddRange(
        [
            A(CdpDomains.Build, "build_structured", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Verify, CdpIntent.Change], 3, 2, "single-flight queue", CsharpLang),
            A(CdpDomains.Build, "run_tests", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Verify], 3, 2, null, CsharpLang),
            A(CdpDomains.Build, "publish_structured", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change, CdpIntent.Ship], 3, 3, null, CsharpLang),
            A(CdpDomains.Build, "get_job_status", ExploreAct.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
            A(CdpDomains.Build, "get_job_log", ActVerify, [CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
            A(CdpDomains.Build, "cancel_job", ActVerify, [CdpObjectKind.Process], [CdpIntent.Change], 1, 2, null, CsharpLang),
        ]);

        return list;
    }

    private static IEnumerable<ToolAffordance> KnowledgeFacet(string domain) =>
    [
        A(domain, "knowledge_tags", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1, "canon tags"),
        A(domain, "read_knowledge_file", AllPhases, [CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
        A(domain, "list_knowledge_files", ExploreClarify, [CdpObjectKind.Kb], [CdpIntent.Find], 2, 1),
        A(domain, "write_knowledge_file", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 3, 4),
        A(domain, "append_knowledge_file", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 2, 3),
        A(domain, "upsert_knowledge_section", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 2, 3),
        A(domain, "validate_sections", ActVerify, [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Verify], 1, 1),
        A(domain, "normalize_sections", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Change, CdpIntent.Verify], 2, 2),
    ];

    private static ToolAffordance A(
        string domain,
        string underlying,
        CdpPhase[] phases,
        CdpObjectKind[] objects,
        CdpIntent[] intents,
        int cost,
        int risk,
        string? hint = null,
        IReadOnlyList<CdpLanguage>? languages = null) =>
        new(
            CdpDomains.Prefixed(domain, underlying),
            domain,
            underlying,
            phases,
            objects,
            intents,
            cost,
            risk,
            hint,
            languages ?? AnyLang);
}
