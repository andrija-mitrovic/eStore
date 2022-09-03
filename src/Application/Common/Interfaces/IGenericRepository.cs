using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
										Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
										string includeString,
										bool disableTracking);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
										Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
										List<Expression<Func<T, object>>>? includes,
										bool disableTracking = true);
		Task<T?> GetByIdAsync(int id);
		Task<T> AddAsync(T entity);
		void UpdateAsync(T entity);
		void DeleteAsync(T entity);
	}
}
