using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public class StatementPrinter
{
    private readonly StatementGenerator _generator;
    private readonly IStatementFormatter _textFormatter;
    private readonly IStatementFormatter _xmlFormatter;

    public StatementPrinter()
        : this(
            new StatementGenerator(),
            new TextStatementFormatter(),
            new XmlStatementFormatter())
    {
    }

    public StatementPrinter(
        StatementGenerator generator,
        IStatementFormatter textFormatter)
        : this(generator, textFormatter, new XmlStatementFormatter())
    {
    }

    public StatementPrinter(
        StatementGenerator generator,
        IStatementFormatter textFormatter,
        IStatementFormatter xmlFormatter)
    {
        _generator = generator;
        _textFormatter = textFormatter;
        _xmlFormatter = xmlFormatter;
    }

    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        var statement = _generator.Generate(invoice, plays);
        return _textFormatter.Format(statement);
    }

    public string PrintXml(Invoice invoice, Dictionary<string, Play> plays)
    {
        var statement = _generator.Generate(invoice, plays);
        return _xmlFormatter.Format(statement);
    }
}
