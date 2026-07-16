using Microsoft.EntityFrameworkCore;
using TheatricalPlayersRefactoringKata.Api.Contracts;
using TheatricalPlayersRefactoringKata.Api.Persistence.Entities;

namespace TheatricalPlayersRefactoringKata.Api.Persistence;

public sealed class StatementRepository(StatementsDbContext dbContext)
{
    public async Task<long> SaveAsync(
        StatementRequest request,
        string format,
        string content,
        CancellationToken cancellationToken)
    {
        var statement = new StoredStatement
        {
            Customer = request.Customer,
            Format = format,
            Content = content,
            CreatedAtUtc = DateTime.UtcNow,
            Plays = request.Plays
                .Select(play => new StoredPlay
                {
                    PlayId = play.Id,
                    Name = play.Name,
                    Lines = play.Lines,
                    Type = play.Type
                })
                .ToList(),
            Performances = request.Performances
                .Select(performance => new StoredPerformance
                {
                    PlayId = performance.PlayId,
                    Audience = performance.Audience
                })
                .ToList()
        };

        dbContext.Statements.Add(statement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return statement.Id;
    }

    public Task<StoredStatement?> GetAsync(
        long id,
        CancellationToken cancellationToken)
    {
        return dbContext.Statements
            .AsNoTracking()
            .Include(statement => statement.Plays)
            .Include(statement => statement.Performances)
            .SingleOrDefaultAsync(
                statement => statement.Id == id,
                cancellationToken);
    }
}
