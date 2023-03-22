using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ums.Core.Base;

namespace Ums.Services.Base
{
    public interface IService<T> where T : BaseEntity
    {
        T Get(object id);
        void InsertOrUpdate(T entity);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);
        IQueryable<T> Gets();
        TU GetColumns<TU>(int id, Expression<Func<T, TU>> columns);
        IQueryable<TU> GetsBy<TU>(Expression<Func<T, TU>> columns);
        IQueryable<TU> GetsBy<TU>(Expression<Func<T, bool>> exp, Expression<Func<T, TU>> columns);
    }
}