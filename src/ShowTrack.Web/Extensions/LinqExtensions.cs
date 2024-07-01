using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Web.Models.Dtos;
using System.Linq.Expressions;

namespace ShowTrack.Web.Extensions;

public static class LinqExtensions
{
    public static async Task<PagedResponseDto<TResult>> FilterAndPaginate<TEntity, TFilter, TResult>(this IQueryable<TEntity> itemsQuery, PagedRequestDto<TFilter> request,
                                                                                                     Expression<Func<TEntity, TResult>> selector) where TFilter : class
    {
        var entityProperties = typeof(TEntity).GetProperties().ToDictionary(p => p.Name, p => p.PropertyType, StringComparer.CurrentCultureIgnoreCase);

        if (request.FilterObj is not null)
        {
            var properties = request.FilterObj is JObject jObject ? jObject.Properties().ToDictionary(p => p.Name, p => (object?)p.Value)
                                                                  : request.FilterObj.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(request.FilterObj));

            var parameter = Expression.Parameter(typeof(TEntity), "item");

            var body = properties.Select(prop => GetExpression(prop, parameter))
                                 .Where(exp => exp is not null)!
                                 .Aggregate<Expression>(Expression.AndAlso);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            itemsQuery = itemsQuery.Where(lambda);
        }

        var resultQuery = itemsQuery.Select(selector);

        if (request.Page is not null && request.Count is not null)
        {
            var totalCount = await resultQuery.CountAsync();

            resultQuery = resultQuery.Skip((request.Page.Value - 1) * request.Count.Value)
                                     .Take(request.Count.Value);


            return new(items: await resultQuery.ToArrayAsync(),
                       totalCount: totalCount,
                       page: request.Page.Value);
        }

        var items = await resultQuery.ToArrayAsync();

        return new(items: items,
                   totalCount: items.Length,
                   page: 1);


        Expression? GetExpression(KeyValuePair<string, object?> prop, ParameterExpression parameter)
        {
            var value = prop.Value;

            if (value == null)
            {
                return null;
            }

            Func<Expression, Expression, BinaryExpression> getExpression;
            int nameStartIndex;

            if (entityProperties.ContainsKey(prop.Key))
            {
                getExpression = Expression.Equal;
                nameStartIndex = 0;
            }
            else if (prop.Key.StartsWith("From", StringComparison.CurrentCultureIgnoreCase))
            {
                getExpression = Expression.GreaterThanOrEqual;
                nameStartIndex = 4;
            }
            else if (prop.Key.StartsWith("To", StringComparison.CurrentCultureIgnoreCase))
            {
                getExpression = Expression.LessThanOrEqual;
                nameStartIndex = 2;
            }
            else
            {
                return null;
            }

            var propertyName = prop.Key[nameStartIndex..];
            if (entityProperties.TryGetValue(propertyName, out var type))
            {
                var valueExpression = Expression.Constant(Convert.ChangeType(value, type));
                return getExpression(Expression.Property(parameter, propertyName), valueExpression);
            }

            return null;
        }
    }
    public static Task<PagedResponseDto<TEntity>> FilterAndPaginate<TEntity, TFilter>(this IQueryable<TEntity> itemsQuery, PagedRequestDto<TFilter> request) where TFilter : class
    {
        return itemsQuery.FilterAndPaginate(request, e => e);
    }
}