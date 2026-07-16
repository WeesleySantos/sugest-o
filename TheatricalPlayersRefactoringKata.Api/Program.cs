using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TheatricalPlayersRefactoringKata;
using TheatricalPlayersRefactoringKata.Api.Contracts;
using TheatricalPlayersRefactoringKata.Api.Mapping;
using TheatricalPlayersRefactoringKata.Api.Persistence;
using TheatricalPlayersRefactoringKata.Api.Persistence.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<StatementPrinter>();
builder.Services.AddDbContext<StatementsDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Statements")));
builder.Services.AddScoped<StatementRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<StatementsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

var statements = app.MapGroup("/statements")
    .WithTags("Statements");

statements.MapPost(
        "/text",
        async (
            StatementRequest request,
            StatementPrinter printer,
            StatementRepository repository,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
            await Generate(
                request,
                printer,
                repository,
                httpContext,
                asXml: false,
                cancellationToken))
    .WithName("GenerateTextStatement")
    .WithSummary("Generates a statement in plain text")
    .Produces(StatusCodes.Status200OK, contentType: "text/plain")
    .ProducesValidationProblem();

statements.MapPost(
        "/xml",
        async (
            StatementRequest request,
            StatementPrinter printer,
            StatementRepository repository,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
            await Generate(
                request,
                printer,
                repository,
                httpContext,
                asXml: true,
                cancellationToken))
    .WithName("GenerateXmlStatement")
    .WithSummary("Generates a statement in XML")
    .Produces(StatusCodes.Status200OK, contentType: "application/xml")
    .ProducesValidationProblem();

statements.MapGet(
        "/{id:long}",
        async (
            long id,
            StatementRepository repository,
            CancellationToken cancellationToken) =>
        {
            var statement = await repository.GetAsync(id, cancellationToken);
            return statement is null
                ? Results.NotFound()
                : Results.Ok(statement);
        })
    .WithName("GetStoredStatement")
    .WithSummary("Gets a persisted statement and its input data")
    .Produces<StoredStatement>()
    .Produces(StatusCodes.Status404NotFound);

app.Run();

static async Task<IResult> Generate(
    StatementRequest request,
    StatementPrinter printer,
    StatementRepository repository,
    HttpContext httpContext,
    bool asXml,
    CancellationToken cancellationToken)
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

        var format = asXml ? "xml" : "text";
        var statementId = await repository.SaveAsync(
            request,
            format,
            result,
            cancellationToken);

        httpContext.Response.Headers["X-Statement-Id"] =
            statementId.ToString(CultureInfo.InvariantCulture);

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
