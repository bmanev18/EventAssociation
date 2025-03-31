using System.Runtime.InteropServices.JavaScript;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventTimesCommand : ICommand
{
    internal EventId Id { get; }
    internal EventTime? StartDate { get; }
    internal EventTime? EndDate { get; }

    private UpdateEventTimesCommand(EventId id, EventTime? startDate, EventTime? endDate)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Result<UpdateEventTimesCommand> Create(string id, string startDate, string endDate)
    {
        var errors = new List<Error>();

        var validId = EventId.Create(id);

        var creatingStartDate = EventTime.Create(startDate);
        var creatingEndDate = EventTime.Create(endDate);
        var results = new List<Result<EventTime>>() { creatingStartDate, creatingEndDate };
        var assertion = Result<EventTime>.AssertResponses(results);

        if (!validId.IsSuccess)
        {
            errors.AddRange(validId.UnwrapErr().ToArray());
        }
        else if (!assertion.IsSuccess)
        {
            errors.AddRange(assertion.UnwrapErr().ToArray());
        }
        else
        {
            return Result<UpdateEventTimesCommand>.Ok(new UpdateEventTimesCommand(validId.Unwrap(),creatingStartDate.Unwrap(),
                        creatingEndDate.Unwrap()));
        }

        return Result<UpdateEventTimesCommand>.Err(errors.ToArray());
    }
}