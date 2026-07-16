using System.Globalization;
using System.Text;

namespace TheatricalPlayersRefactoringKata;

public class TextStatementFormatter : IStatementFormatter
{
    private static readonly CultureInfo Culture = new("en-US");

    public string Format(Statement statement)
    {
        var result = new StringBuilder();
        result.Append("Statement for ")
            .Append(statement.Customer)
            .Append('\n');

        foreach (var line in statement.Lines)
        {
            result.AppendFormat(
                Culture,
                "  {0}: {1:C} ({2} seats)\n",
                line.PlayName,
                line.Amount / 100m,
                line.Seats);
        }

        result.AppendFormat(
            Culture,
            "Amount owed is {0:C}\n",
            statement.TotalAmount / 100m);
        result.Append("You earned ")
            .Append(statement.TotalCredits)
            .Append(" credits\n");

        return result.ToString();
    }
}
