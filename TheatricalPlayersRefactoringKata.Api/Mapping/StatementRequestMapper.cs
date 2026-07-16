using TheatricalPlayersRefactoringKata.Api.Contracts;

namespace TheatricalPlayersRefactoringKata.Api.Mapping;

public static class StatementRequestMapper
{
    public static (Invoice Invoice, Dictionary<string, Play> Plays) Map(
        StatementRequest request)
    {
        var plays = request.Plays.ToDictionary(
            play => play.Id,
            play => new Play(play.Name, play.Lines, play.Type));

        var performances = request.Performances
            .Select(performance =>
                new Performance(
                    performance.PlayId,
                    performance.Audience))
            .ToList();

        return (new Invoice(request.Customer, performances), plays);
    }

    public static Dictionary<string, string[]> Validate(
        StatementRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Customer))
        {
            errors["customer"] = ["Customer is required."];
        }

        if (request.Plays is null || request.Plays.Count == 0)
        {
            errors["plays"] = ["At least one play is required."];
        }
        else if (request.Plays.Any(play => string.IsNullOrWhiteSpace(play.Id)))
        {
            errors["plays"] = ["Every play must have an id."];
        }
        else if (request.Plays.GroupBy(play => play.Id).Any(group => group.Count() > 1))
        {
            errors["plays"] = ["Play ids must be unique."];
        }

        if (request.Performances is null || request.Performances.Count == 0)
        {
            errors["performances"] = ["At least one performance is required."];
        }
        else if (request.Performances.Any(performance => performance.Audience < 0))
        {
            errors["performances"] = ["Audience cannot be negative."];
        }

        if (request.Plays is not null && request.Performances is not null)
        {
            var playIds = request.Plays
                .Select(play => play.Id)
                .ToHashSet();

            if (request.Performances.Any(performance =>
                    !playIds.Contains(performance.PlayId)))
            {
                errors["performances"] =
                    ["Every performance must reference an existing play."];
            }
        }

        return errors;
    }
}
