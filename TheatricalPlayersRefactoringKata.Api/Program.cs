using TheatricalPlayersRefactoringKata;
using TheatricalPlayersRefactoringKata.Api.Contracts;
using TheatricalPlayersRefactoringKata.Api.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<StatementPrinter>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var statements = app.MapGroup("/statements")
    .WithTags("Statements");

statements.MapPost(
        "/text",
        (StatementRequest request, StatementPrinter printer) =>
            Generate(request, printer, asXml: false))
    .WithName("GenerateTextStatement")
    .WithSummary("Generates a statement in plain text")
    .Produces(StatusCodes.Status200OK, contentType: "text/plain")
    .ProducesValidationProblem();

statements.MapPost(
        "/xml",
        (StatementRequest request, StatementPrinter printer) =>
            Generate(request, printer, asXml: true))
    .WithName("GenerateXmlStatement")
    .WithSummary("Generates a statement in XML")
    .Produces(StatusCodes.Status200OK, contentType: "application/xml")
    .ProducesValidationProblem();

app.Run();

static IResult Generate(
    StatementRequest request,
    StatementPrinter printer,
    bool asXml)
{
    var errors = StatementRequestMapper.Validate(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    var (invoice, plays) = StatementRequestMapper.Map(request);

    try
    {
        var result = asXml
            ? printer.PrintXml(invoice, plays)
            : printer.Print(invoice, plays);

        var contentType = asXml
            ? "application/xml; charset=utf-8"
            : "text/plain; charset=utf-8";

        return Results.Text(result, contentType);
    }
    catch (Exception exception)
        when (exception.Message.StartsWith(
            "unknown type:",
            StringComparison.Ordinal))
    {
        return Results.ValidationProblem(
            new Dictionary<string, string[]>
            {
                ["plays"] = [exception.Message]
            });
    }
}
