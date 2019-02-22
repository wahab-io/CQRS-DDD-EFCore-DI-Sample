using System;
using System.Linq;
using System.Linq.Expressions;

namespace todo_cqrs.Core
{
    /// <summary>
    /// Generic Repository Pattern
    /// </summary>
    public interface IRepository<T> 
                    where T : class, IEntity
    {
        IQueryable<T> FindAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T FindById(int id);
        void Add(T newEntity);
        void Remove(T entity);
    }
}