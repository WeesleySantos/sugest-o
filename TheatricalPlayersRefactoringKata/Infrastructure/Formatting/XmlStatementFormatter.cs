using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace TheatricalPlayersRefactoringKata;

public class XmlStatementFormatter : IStatementFormatter
{
    public string Format(Statement statement)
    {
        var output = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace
        };

        using (var stringWriter = new Utf8StringWriter(output))
        using (var writer = XmlWriter.Create(stringWriter, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Statement");
            writer.WriteAttributeString(
                "xmlns",
                "xsi",
                null,
                "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString(
                "xmlns",
                "xsd",
                null,
                "http://www.w3.org/2001/XMLSchema");

            writer.WriteElementString("Customer", statement.Customer);
            writer.WriteStartElement("Items");

            foreach (var line in statement.Lines)
            {
                writer.WriteStartElement("Item");
                writer.WriteElementString("AmountOwed", FormatAmount(line.Amount));
                writer.WriteElementString(
                    "EarnedCredits",
                    line.Credits.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString(
                    "Seats",
                    line.Seats.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteElementString(
                "AmountOwed",
                FormatAmount(statement.TotalAmount));
            writer.WriteElementString(
                "EarnedCredits",
                statement.TotalCredits.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        return output.ToString();
    }

    private static string FormatAmount(int amountInCents)
    {
        return (amountInCents / 100m).ToString(
            "0.##",
            CultureInfo.InvariantCulture);
    }

    private sealed class Utf8StringWriter : StringWriter
    {
        public Utf8StringWriter(StringBuilder builder)
            : base(builder, CultureInfo.InvariantCulture)
        {
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
