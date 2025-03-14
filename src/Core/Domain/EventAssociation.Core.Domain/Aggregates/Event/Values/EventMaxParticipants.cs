    using EventAssociation.Core.Domain.Common.Values;
    using EventAssociation.Core.Tools.OperationResult;

    namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

    public class EventMaxParticipants: ValueObject
    {
        internal int Value { get;}
        private EventMaxParticipants(int value)
        {
            Value = value;
        }


        public static Result<EventMaxParticipants> Create(int value)
        {
            var validationResult = Validate(value);
            var errors =  validationResult
                .Where(r => !r.IsSuccess)
                .SelectMany(r => r.UnwrapErr()) 
                .ToList();

            if (errors.Any())
            {
                return Result<EventMaxParticipants>.Err(errors.ToArray());
            }

            return Result<EventMaxParticipants>.Ok(new EventMaxParticipants(value));
        }
        
        private static List<Result<None>> Validate(int value)
        {
            var results = new List<Result<None>>();
        
            var underLimitResult = ValidateMaxParticipantsIsNotUnderTheLimit(value);
            var aboveLimitResult = ValidateMaxParticipantsIsNotAboveTheLimit(value);
            
            results.Add(underLimitResult);
            results.Add(aboveLimitResult);
            return results;
        }


        private static Result<None> ValidateMaxParticipantsIsNotUnderTheLimit(int value)
        {
            if (value < 5)
            {
            return Result<None>.Err(new Error("", "The max participants cannot be less than 5"));
            }
            
            return Result<None>.Ok(None.Value);
        }
        
        private static Result<None> ValidateMaxParticipantsIsNotAboveTheLimit(int value)
        {
            if (value > 50)
            {
                return Result<None>.Err(new Error("", "The max participants cannot be greater than 50"));
            }
            return Result<None>.Ok(None.Value);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }