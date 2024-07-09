using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShowTrack.Contracts.Dtos;
using ShowTrack.Web.Models.Dtos;
using System.Linq.Expressions;
using System.Reflection;

namespace ShowTrack.Web.Extensions;

public static class LinqExtensions
{
    private static readonly Dictionary<string, MethodInfo> _orderMethods;

    static LinqExtensions()
    {
        _orderMethods = typeof(Queryable)
                        .GetMethods()
                        .Where(m => (m.Name.StartsWith("OrderBy") || m.Name.StartsWith("ThenBy")) && m.GetParameters().Length == 2)
                        .ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
    }

    public static async Task<PagedResponseDto<TResult>> FilterAndPaginate<TEntity, TFilter, TResult>(this IQueryable<TEntity> itemsQuery, PagedRequestDto<TFilter> request,
                                                                                                     Expression<Func<TEntity, TResult>> selector) where TFilter : class
    {
        var entityProperties = typeof(TEntity).GetProperties().ToDictionary(p => p.Name, p => p.PropertyType, StringComparer.CurrentCultureIgnoreCase);

        var entityParameter = Expression.Parameter(typeof(TEntity), "item");

        if (request.FilterObj is not null)
        {
            var properties = request.FilterObj is JObject jObject ? jObject.Properties().ToDictionary(p => p.Name, p => (object?)p.Value)
                                                                  : request.FilterObj.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(request.FilterObj));

            var body = properties.Select(prop => GetFilterExpression(prop, entityParameter))
                                 .Where(exp => exp is not null)!
                                 .Aggregate<Expression>(Expression.AndAlso);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, entityParameter);
            itemsQuery = itemsQuery.Where(lambda);
        }

        if (request.SortItems is { Length: > 0 })
        {

            for (var i = 0; i < request.SortItems.Length; i++)
            {
                var sortMember = GetOrderSortExpression(request.SortItems[i], entityParameter, out var isDescending);

                if (sortMember is null)
                {
                    continue;
                }

                var methodName = i == 0 ? "OrderBy" : "ThenBy";
                methodName += isDescending ? "Descending" : string.Empty;

                itemsQuery = DynamicOrderQuery(itemsQuery, entityParameter, sortMember, methodName) ?? itemsQuery;
            }
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


        Expression? GetFilterExpression(KeyValuePair<string, object?> prop, ParameterExpression parameter)
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

        MemberExpression? GetOrderSortExpression(string propertyName, ParameterExpression parameter, out bool isDescending)
        {
            isDescending = false;

            if (propertyName.EndsWith("Desc", StringComparison.InvariantCultureIgnoreCase))
            {
                isDescending = true;
                propertyName = propertyName[..^4].Trim();
            }

            return entityProperties.ContainsKey(propertyName) ? Expression.Property(parameter, propertyName) : null;
        }
    }
    public static Task<PagedResponseDto<TEntity>> FilterAndPaginate<TEntity, TFilter>(this IQueryable<TEntity> itemsQuery, PagedRequestDto<TFilter> request) where TFilter : class
    {
        return itemsQuery.FilterAndPaginate(request, e => e);
    }

    private static IOrderedQueryable<T>? DynamicOrderQuery<T>(this IQueryable<T> query, ParameterExpression entity, MemberExpression orderProperty, string nativeMethod)
    {
        var selector = Expression.Lambda(orderProperty, entity);

        var method = _orderMethods.GetValueOrDefault(nativeMethod) ?? throw new ArgumentException("Invalid Method Name", nameof(nativeMethod));

        var genericMethod = method.MakeGenericMethod(typeof(T), orderProperty.Type);

        return (IOrderedQueryable<T>?)genericMethod.Invoke(genericMethod, [query, selector]);
    }
}