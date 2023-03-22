using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Ums.Core.Base;
using Ums.Core.Data;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Services.Base
{
    public abstract class Service<T> : IService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        private readonly ISystemLogService _logService;
        protected Service(DbContext context, IContextService contextService)
        {
            _repository = new Repository<T>(context);
            _logService = contextService.Resolve<ISystemLogService>();
        }
        public virtual T Get(object id)
        {
            return _repository.Get(id);
        }
        public void InsertOrUpdate(T entity)
        {
            if (entity.Id > 0)
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
        }
        public virtual void Insert(T entity)
        {
            _repository.Insert(entity);
            _logService.Log("INSERT " + typeof(T).FullName, entity);
        }
        public virtual void InsertRange(IEnumerable<T> entities)
        {
            _repository.Insert(entities);
        }
        public virtual void Update(T entity)
        {
            _repository.Update(entity);
            _logService.Log("UPDATE " + typeof(T).FullName, entity);
        }
        public virtual void Delete(object id)
        {
            var entity = Get(id);
            Delete(entity);
        }
        public virtual void Delete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
            _logService.Log("DELETE " + typeof(T).FullName, entity);
        }
        public virtual void HardDelete(T entity)
        {
            _repository.Delete(entity);
            _logService.Log("DELETE " + typeof(T).FullName, entity);
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