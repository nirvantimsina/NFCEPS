using FluentValidation;
using MediatR;

namespace NFCEPS.Application.Behavior
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .Select(f => new ValidationErrorDetails
                {
                    OccuredIn = f.PropertyName.Split('.').Last(),
                    ErrorCode = f.ErrorCode
                })
                .ToList();

            if (failures.Count != 0)
            {
                throw new CustomValidationException(failures);
            }

            return await next();
        }
    }
}

public record ValidationErrorDetails
{
    public string? OccuredIn { get; set; }
    public string? ErrorCode { get; set; }
}

public class CustomValidationException : Exception
{
    public List<ValidationErrorDetails> Errors { get; }
    public CustomValidationException(List<ValidationErrorDetails> errors) => Errors = errors;
}