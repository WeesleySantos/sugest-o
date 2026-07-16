namespace TheatricalPlayersRefactoringKata.Api.Persistence.Entities;

public sealed class StoredStatement
{
    public long Id { get; set; }

    public string Customer { get; set; } = string.Empty;

    public string Format { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public List<StoredPlay> Plays { get; set; } = [];

    public List<StoredPerformance> Performances { get; set; } = [];
}

public sealed class StoredPlay
{
    public long Id { get; set; }

    public long StatementId { get; set; }

    public string PlayId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Lines { get; set; }

    public string Type { get; set; } = string.Empty;
}

public sealed class StoredPerformance
{
    public long Id { get; set; }

    public long StatementId { get; set; }

    public string PlayId { get; set; } = string.Empty;

    public int Audience { get; set; }
}
