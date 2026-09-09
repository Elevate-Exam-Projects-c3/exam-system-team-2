using FluentValidation;
using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            // If TResponse is RequestResponse<T>, return Fail with 400 and validation errors
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(RequestResponse<>))
            {
                var failMethod = typeof(TResponse).GetMethod("Fail", new[] { typeof(string), typeof(int), typeof(IDictionary<string, string[]>) });
                if (failMethod != null)
                {
                    return (TResponse)failMethod.Invoke(null, new object?[] { "Validation failed", 400, errors })!;
                }
            }

            if (typeof(TResponse) == typeof(RequestResponse))
            {
                return (TResponse)(object)RequestResponse.Fail("Validation failed", 400, errors);
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
