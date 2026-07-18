namespace Cdp.Core;

/// <summary>Seed affordances for wave1 backends (prefixed names).</summary>
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

    public static IReadOnlyList<ToolAffordance> Build() =>
    [
        // Agent Notes
        A("an", "knowledge_tags", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1, "canon tags"),
        A("an", "read_knowledge_file", AllPhases, [CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
        A("an", "route_context", ExploreClarify, [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Find], 2, 1),
        A("an", "read_hot_context", ExploreClarify.Concat([CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
        A("an", "memory_health", [CdpPhase.Explore, CdpPhase.Verify], [CdpObjectKind.Session], [CdpIntent.Verify], 1, 1),
        A("an", "list_knowledge_files", ExploreClarify, [CdpObjectKind.Kb], [CdpIntent.Find], 2, 1),
        A("an", "search_agent_notes", ExploreClarify, [CdpObjectKind.Session, CdpObjectKind.Kb], [CdpIntent.Find], 2, 1),
        A("an", "write_knowledge_file", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 3, 4),
        A("an", "append_knowledge_file", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 2, 3),
        A("an", "upsert_knowledge_section", [CdpPhase.Act], [CdpObjectKind.Kb], [CdpIntent.Change, CdpIntent.Record], 2, 3),
        A("an", "upsert_agent_notes_section", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Session], [CdpIntent.Change, CdpIntent.Record], 2, 3),
        A("an", "validate_sections", ActVerify, [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Verify], 1, 1),
        A("an", "normalize_sections", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Kb, CdpObjectKind.Session], [CdpIntent.Change, CdpIntent.Verify], 2, 2),

        // Task Knowledge
        A("tk", "man", AllPhases, [CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
        A("tk", "ensure_store", ExploreAct, [CdpObjectKind.Task], [CdpIntent.Change], 2, 2),
        A("tk", "route_next", ExploreClarify.Concat([CdpPhase.Act, CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Task], [CdpIntent.Find, CdpIntent.Ship], 1, 1),
        A("tk", "tasks", ExploreClarify, [CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
        A("tk", "task_upsert", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Task], [CdpIntent.Change, CdpIntent.Record], 2, 2),
        A("tk", "read_card", ExploreClarify.Concat([CdpPhase.Act]).ToArray(), [CdpObjectKind.Task], [CdpIntent.Find, CdpIntent.Cite], 1, 1),
        A("tk", "write_card", [CdpPhase.Act], [CdpObjectKind.Task], [CdpIntent.Change], 3, 3),
        A("tk", "upsert_section", [CdpPhase.Act], [CdpObjectKind.Task], [CdpIntent.Change, CdpIntent.Record], 2, 2),
        A("tk", "analytics_upsert", [CdpPhase.Act, CdpPhase.Handoff], [CdpObjectKind.Task], [CdpIntent.Record], 2, 2),

        // Findings
        A("find", "man", AllPhases, [CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
        A("find", "findings", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Find], 1, 1),
        A("find", "finding_record", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Record], 2, 2),
        A("find", "finding_check", ActVerify, [CdpObjectKind.Finding, CdpObjectKind.Code], [CdpIntent.Verify], 1, 1),
        A("find", "tasks", ExploreClarify, [CdpObjectKind.Finding, CdpObjectKind.Task], [CdpIntent.Find], 1, 1),
        A("find", "task_record", [CdpPhase.Act], [CdpObjectKind.Finding, CdpObjectKind.Task], [CdpIntent.Record], 2, 2),

        // Failures
        A("fail", "man", AllPhases, [CdpObjectKind.Process, CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
        A("fail", "failures", ExploreClarify.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Process, CdpObjectKind.Finding], [CdpIntent.Find], 1, 1),
        A("fail", "failure_record", [CdpPhase.Act, CdpPhase.Verify], [CdpObjectKind.Process], [CdpIntent.Record], 2, 2),

        // Dotnet Debug (C# stack)
        A("dbg", "man", AllPhases, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find], 1, 1, "ops: stop before rebuild", CsharpLang),
        A("dbg", "debug_ping", ExploreAct.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Process], [CdpIntent.Verify], 1, 1, null, CsharpLang),
        A("dbg", "debug_set_breakpoints", [CdpPhase.Act], [CdpObjectKind.Code], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_list_breakpoints", ExploreAct.Concat([CdpPhase.Verify]).ToArray(), [CdpObjectKind.Code], [CdpIntent.Find], 1, 1, null, CsharpLang),
        A("dbg", "debug_clear_breakpoints", [CdpPhase.Act], [CdpObjectKind.Code], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_launch", [CdpPhase.Act], [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 3, 4, null, CsharpLang),
        A("dbg", "debug_attach", [CdpPhase.Act], [CdpObjectKind.Process], [CdpIntent.Change], 3, 4, null, CsharpLang),
        A("dbg", "debug_continue", ActVerify, [CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_step_over", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_step_into", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_step_out", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Change], 2, 2, null, CsharpLang),
        A("dbg", "debug_stop", ActVerify.Concat([CdpPhase.Handoff]).ToArray(), [CdpObjectKind.Process], [CdpIntent.Change], 1, 2, "prefer over taskkill", CsharpLang),
        A("dbg", "debug_stop_context", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, "after stopped", CsharpLang),
        A("dbg", "debug_stack_trace", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
        A("dbg", "debug_variables", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
        A("dbg", "debug_variable_children", ActVerify, [CdpObjectKind.Code, CdpObjectKind.Process], [CdpIntent.Find, CdpIntent.Verify], 1, 1, null, CsharpLang),
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
            $"{domain}_{underlying}",
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
