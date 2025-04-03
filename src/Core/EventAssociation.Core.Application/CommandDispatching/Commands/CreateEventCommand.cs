using EventAssociation.Core.Domain.Aggregates.Event.Bases;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class CreateEventCommand : ICommand
{
    internal EventTime? StartDate { get; }
    internal EventTime? EndDate { get; }
    internal EventType Type { get; }
    internal Location Location_ { get; }

    private CreateEventCommand(EventTime? startDate, EventTime? endDate, EventType type, Location location)
    {
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Location_ = location;
    }

    public static Result<CreateEventCommand> Create(string startDate, string endDate, EventType eventType, LocationType locationType,
        string locationName, int locationCapacity)
    {
        var errors = new List<Error>();

        var creatingStartDate = EventTime.Create(startDate);
        var creatingEndDate = EventTime.Create(endDate);

        var lName = LocationName.Create(locationName);
        var lCapacity = LocationCapacity.Create(locationCapacity);

        if (!creatingEndDate.IsSuccess)
        {
            errors.AddRange(creatingStartDate.UnwrapErr());
        }else if (!creatingStartDate.IsSuccess)
        {
            errors.AddRange(creatingEndDate.UnwrapErr());
        }
        else if (!lName.IsSuccess)
        {
            errors.AddRange(lName.UnwrapErr());
        }
        else if (!lCapacity.IsSuccess)
        {
            errors.AddRange(lCapacity.UnwrapErr());
        }
        else
        {
            var location = Location.CreateLocation(locationType, lName.Unwrap(), lCapacity.Unwrap());
            if (!location.IsSuccess)
            {
                errors.AddRange(location.UnwrapErr());
            }
            else
            {
                 var command = new CreateEventCommand(creatingStartDate.Unwrap(), creatingEndDate.Unwrap(), eventType, location.Unwrap());
                 return Result<CreateEventCommand>.Ok(command);
            }

        }

        return Result<CreateEventCommand>.Err(errors.ToArray());}
}