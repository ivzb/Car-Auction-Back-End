namespace WebService.Infrastructure.Automapper
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper.QueryableExtensions;

    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(this IQueryable source, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return source.ProjectTo(AutoMapperConfig.Configuration, membersToExpand);
        }

        public static IQueryable<TDestination> To<TDestination>(this IQueryable source, object parameters, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return source.ProjectTo(AutoMapperConfig.Configuration, parameters, membersToExpand);
        }
    }
}