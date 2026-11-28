using FluentValidation;

namespace GameBacklog.Validation;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // Find the argument whose type matches T.
        var model = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (model is null)
        {
            return Results.BadRequest("Expected request data is missing.");
        }

        // Ask DI for a validator for that type.
        var validator = context.HttpContext.RequestServices
            .GetRequiredService<IValidator<T>>();

        var validation = await validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        // Validation passed: run the next filter or endpoint.
        return await next(context);
    }
}