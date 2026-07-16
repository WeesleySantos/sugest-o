namespace TheatricalPlayersRefactoringKata.Api.Contracts;

public sealed record StatementRequest(
    string Customer,
    IReadOnlyList<PlayRequest> Plays,
    IReadOnlyList<PerformanceRequest> Performances);

public sealed record PlayRequest(
    string Id,
    string Name,
    int Lines,
    string Type);

public sealed record PerformanceRequest(
    string PlayId,
    int Audience);
