using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event.Contracts;

public delegate Result<None> AreTheSameDelegate<T>(T other);
public delegate Result<None> AtLeastOneHourDifferenceDelegate<T>(T other);
public delegate Result<None> IntervalLessThan10HoursDelegate<T>(T other);
public delegate Result<None> After8Delegate();
public delegate Result<None> Before2359Delegate();
public delegate Result<None> Before1AmDelegate();
public delegate Result<None> IsNextDayFromDelegate<T>(T other);
public delegate Result<None> LaterThanNowDelegate();





