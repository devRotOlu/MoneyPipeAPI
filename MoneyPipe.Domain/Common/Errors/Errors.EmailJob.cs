using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class EmailJob
        {
            public static Error EmailRequired => Error.Validation(code: "EmailJob.EmailRequired",
             description: "An email job must specify a recipient email address.");
            public static Error SubjectRequired => Error.Validation(code: "EmailJob.SubjectRequired",
             description: "An email job must include a subject line.");
            public static Error MessageRequired => Error.Validation(code: "EmailJob.MessageRequired",
             description: "An email job must contain a message body.");
        }
    }   
}