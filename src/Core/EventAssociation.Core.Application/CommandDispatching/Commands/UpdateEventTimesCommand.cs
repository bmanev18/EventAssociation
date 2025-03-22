using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventTimesCommand
{
    internal EventId Id { get; }
    internal EventTime? StartDate { get; }
    internal EventTime? EndDate { get; }

    private UpdateEventTimesCommand(EventTime? startDate, EventTime? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Result<UpdateEventTimesCommand> Create(string id, string startDate, string endDate)
    {
        var creatingStartDate = EventTime.Create(startDate);
        var creatingEndDate = EventTime.Create(endDate);

        var results = new List<Result<EventTime>>() { creatingStartDate, creatingEndDate };
        var assertion = Result<EventTime>.AssertResponses(results);

        if (!assertion.IsSuccess)
        {
            return Result<UpdateEventTimesCommand>.Err(assertion.UnwrapErr().ToArray());
        }

        return Result<UpdateEventTimesCommand>.Ok(new UpdateEventTimesCommand(creatingStartDate.Unwrap(),
            creatingEndDate.Unwrap()));
    }
}