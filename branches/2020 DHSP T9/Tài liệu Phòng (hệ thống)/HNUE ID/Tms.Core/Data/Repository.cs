using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using Ums.Core.Base;

namespace Ums.Core.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private IDbSet<T> _entities;
        public Repository(DbContext context)
        {
            _context = context;
        }
        public T Get(dynamic id)
        {
            return Entities.Find(id);
        }
        public int Insert(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                Entities.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine));
                throw new Exception(msg, dbEx);
            }
        }
        public void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException(nameof(entities));
                foreach (var entity in entities) Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + (string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine));
                throw new Exception(msg, dbEx);
            }
        }
        public int Update(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                Entities.AddOrUpdate(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));
                throw new Exception(msg, dbEx);
            }
        }
        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException(nameof(entities));
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + (string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine));
                throw new Exception(msg, dbEx);
            }
        }
        public void Delete(T entity)
        {
            try
            {
                if (entity == null) return;
                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));
                throw new Exception(msg, dbEx);
            }
        }
        public void Delete(dynamic id)
        {
            Delete(Get(id));
        }
        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException(nameof(entities));
                foreach (var entity in entities) Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate(string.Empty, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));
                throw new Exception(msg, dbEx);
            }
        }
        public IQueryable<T> Table => Entities;
        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();
        protected virtual IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());
    }
}