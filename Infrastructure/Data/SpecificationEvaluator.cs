
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
                
            }

            if(spec.IsDistinct)
            {
                query = query.Distinct();
            }

            if (spec.IsPagingEnabled)
            {
                 query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => 
            current.Include(include));

            return query;
        }


        public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> inputQuery,
        ISpecification<T,TResult> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
                
            }

            query = spec.Includes.Aggregate(query, (current, include) => 
            current.Include(include));

            var selectedQuery  = query as IQueryable<TResult>;

            if(spec.Select != null)
            {
                selectedQuery = query.Select(spec.Select);
            }

            if (spec.IsDistinct)
            {
                selectedQuery = selectedQuery?.Distinct();
            }
            if (spec.IsPagingEnabled)
            {
                 selectedQuery = selectedQuery?.Skip(spec.Skip).Take(spec.Take);
            }

            return selectedQuery ?? query.Cast<TResult>();
            
        }



    }
}