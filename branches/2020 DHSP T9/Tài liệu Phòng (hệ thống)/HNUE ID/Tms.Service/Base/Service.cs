using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using Ums.Core.Base;
using Ums.Core.Data;
using Ums.Services.Security;

namespace Ums.Services.Base
{
    public abstract class Service<T> : IService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        private readonly IContextService _contextService;
        protected Service(DbContext context, IContextService contextService)
        {
            _contextService = contextService;
            _repository = new Repository<T>(context);
        }
        public virtual T Get(object id)
        {
            return _repository.Get(id);
        }
        public int InsertOrUpdate(T entity)
        {
            if (entity.Id > 0)
            {
                return _repository.Update(entity);
            }
            else
            {
                return _repository.Insert(entity);
            }
        }
        public virtual int Insert(T entity)
        {
            return _repository.Insert(entity);
        }
        public virtual void InsertRange(IEnumerable<T> entities)
        {
            _repository.Insert(entities);
        }
        public virtual int Update(T entity)
        {
            return _repository.Update(entity);
        }
        public virtual void Delete(object id)
        {
            Delete(Get(id));
        }
        public virtual void Delete(T entity)
        {
            if (entity == null) return;
            entity.IsDeleted = true;
            Update(entity);
        }
        public virtual void HardDelete(T entity)
        {
            var log = LogManager.GetLogger(typeof(T).FullName);
            _repository.Delete(entity);
        }
        public virtual IQueryable<T> Gets()
        {
            return _repository.Table.Where(i => !i.IsDeleted);
        }
        public TU GetColumns<TU>(int id, Expression<Func<T, TU>> columns)
        {
            return GetsBy(i => i.Id == id, columns).FirstOrDefault();
        }
        public IQueryable<TU> GetsBy<TU>(Expression<Func<T, TU>> columns)
        {
            return Gets().Select(columns);
        }
        public IQueryable<TU> GetsBy<TU>(Expression<Func<T, bool>> exp, Expression<Func<T, TU>> columns)
        {
            return Gets().Where(exp).Select(columns);
        }
    }
}