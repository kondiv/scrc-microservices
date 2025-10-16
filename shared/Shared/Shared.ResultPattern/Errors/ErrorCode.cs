namespace Shared.ResultPattern.Errors;

public enum ErrorCode
{
    DbUpdate,
    NotFound,
    DbUpdateConcurrency,
    Conflict,
    AlreadyExists,
    AuthProblem,
    Validation
}