using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Cloudey.FluentValidation.Rules;

public static class ExistsRule
{
    /// <summary>
    ///     Check if an entity with the given property exists in the database
    /// </summary>
    /// <param name="ruleBuilder"></param>
    /// <param name="dbContext">Database context to use for looking up the entity</param>
    /// <param name="entityExpression">Member access expression for the entity property (e.g. (User user) => user.Email)</param>
    /// <param name="valueTransformation">Optional delegate applied to the value under validation before comparison</param>
    /// <typeparam name="T">Type of the object under validation</typeparam>
    /// <typeparam name="TProperty">Type of the property to be validated</typeparam>
    /// <typeparam name="TEntity">Type of the entity to check against</typeparam>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TProperty> Exists<T, TProperty, TEntity> (
        this IRuleBuilder<T, TProperty> ruleBuilder,
        DbContext dbContext,
        Expression<Func<TEntity, TProperty>> entityExpression,
        Func<TProperty, TProperty>? valueTransformation = null
    )
        where TEntity : class
    {
        return ruleBuilder.SetAsyncValidator(
                new ExistsValidator<T, TProperty, TEntity>(dbContext, entityExpression, valueTransformation)
            )
            .WithMessage("Must exist");
    }
}

public class ExistsValidator<T, TProperty, TEntity> : AsyncPropertyValidator<T, TProperty>
    where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly Expression<Func<TEntity, TProperty>> _property;
    private readonly Func<TProperty, TProperty>? _valueTransformation;

    public ExistsValidator (
        DbContext dbContext,
        Expression<Func<TEntity, TProperty>> property,
        Func<TProperty, TProperty>? valueTransformation = null
    )
    {
        _property = property;
        _dbContext = dbContext;
        _valueTransformation = valueTransformation;
    }

    public override string Name => "ExistsValidator";

    public override async Task<bool> IsValidAsync (
        ValidationContext<T> context,
        TProperty value,
        CancellationToken cancellation
    )
    {
        var db = _dbContext.Set<TEntity>();

        var transformedValue = _valueTransformation is null ? value : _valueTransformation(value);

        var valueExpression = Expression.Constant(transformedValue);
        var parameter = Expression.Parameter(typeof(TEntity));
        var propertyExpression = Expression.Property(parameter, _property.GetPropertyAccess());
        var equalityExpression =
            Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(propertyExpression, valueExpression),
                parameter
            );

        return await db.AnyAsync(equalityExpression, cancellation);
    }
}