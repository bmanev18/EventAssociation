using System.Dynamic;
using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event.Values;

public class EventTime: ValueObject
{
    private DateTime Value { get; }
    
    public EventTime(DateTime value)
    {
        Value = value;
    }

    public static Result<EventTime> Create(string time)
    {
        try
        {
            var result = DateTime.Parse(time);
            var newTime = new EventTime(result);
            return Result<EventTime>.Ok(newTime);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<EventTime>.Err(new Error("Parse", e.Message));
        }
    }
    public Result<None> IsBefore(EventTime other)
    {
        return Value < other.Value
            ? Result<None>.Ok(None.Value)
            : Result<None>.Err(new Error("IS_NOT_BEFORE", "Value is not before the other value"));
    }
    public Result<None> isTheSameDayAs(EventTime other)
    {
        return Value.Date == other.Value.Date 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("TIMES_ARE_DIFFERENT", "Times are not the same"));
    }

    public Result<None> AtLeastOneHourSince(EventTime other)
    {
        var difference = (Value - other.Value).Duration().TotalHours;
        return difference >= 1 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("LESS_THAN_ONE_HOUR", "Difference is less than one hour"));
    }

    public Result<None> IntervalLessThan10Hours(EventTime other)
    {
        var difference = (Value - other.Value).Duration().TotalHours;
        return difference < 10 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("MORE_THAN_TEN_HOURS", "Interval is greater than 10 hours"));
    }

    public Result<None> After8()
    {
        return Value.Hour >= 8 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("BEFORE_EIGHT", "Time is before 8 AM"));
    }

    public Result<None> Before12Am()
    {
        return Value.Hour < 23 || (Value.Hour == 23 && Value.Minute == 0) 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("AFTER_2359", "Time is after 23:59"));
    }
    
    public Result<None> Before1Am()
    {
        return Value.Hour < 1 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("AFTER_1AM", "Time is after 1 AM"));
    }

    public Result<None> IsNextDayFrom(EventTime other)
    {
        var nextDayFromOther = other.Value.AddDays(1);  // Start of the next day (00:00)
        return Value.Date == nextDayFromOther.Date 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("NOT_NEXT_DAY", "Time is not the next day"));
    }
    
    public Result<None> LaterThanNow()
    {
        var now = DateTime.Now;
        return Value > now 
            ? Result<None>.Ok(None.Value) 
            : Result<None>.Err(new Error("NOT_LATER_THAN_NOW", "Date is not later than the current time"));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}