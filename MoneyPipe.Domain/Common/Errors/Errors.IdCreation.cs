using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {        
        public static class IdCreation
        {      
            public static Error InvalidId(object providedValue) =>
                Error.Validation(
                    code: $"{providedValue}.InvalidId",
                    description: $"Invalid Id"
                );
        
        }
    }
}