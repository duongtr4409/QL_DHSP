using System.Collections.Generic;
using System.Linq;
using Ums.Core.Base;

namespace Ums.Core.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Get(dynamic id);
        int Insert(T entity);
        void Insert(IEnumerable<T> entities);
        int Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(dynamic id);
        void Delete(IEnumerable<T> entities);
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}