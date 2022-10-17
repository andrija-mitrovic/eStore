using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
		Task<IReadOnlyList<T>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
										bool disableTracking = true,
										CancellationToken cancellationToken = default);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
										Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
										string? includeString,
										bool disableTracking = true,
										CancellationToken cancellationToken = default);
		Task<IReadOnlyList<T?>> GetAsync(Expression<Func<T, bool>>? predicate,
										 Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
										 List<Expression<Func<T, object>>>? includes,
										 bool disableTracking = true,
										 CancellationToken cancellationToken = default);
		Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
		void Update(T entity);
		void Delete(T entity);
	}
}
