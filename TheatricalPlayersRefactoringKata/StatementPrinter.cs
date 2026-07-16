using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public class StatementPrinter
{
    private readonly StatementGenerator _generator;
    private readonly IStatementFormatter _formatter;

    public StatementPrinter()
        : this(new StatementGenerator(), new TextStatementFormatter())
    {
    }

    public StatementPrinter(
        StatementGenerator generator,
        IStatementFormatter formatter)
    {
        _generator = generator;
        _formatter = formatter;
    }

    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        var statement = _generator.Generate(invoice, plays);
        return _formatter.Format(statement);
    }
}
