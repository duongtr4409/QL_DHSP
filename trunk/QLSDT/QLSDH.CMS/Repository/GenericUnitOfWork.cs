﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEMIS.Model;

namespace TEMIS.CMS.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private TEMIS_systemEntities DBEntity = new TEMIS_systemEntities();

        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
            return new GenericRepository<Tbl_EntityType>(DBEntity);
        }

        public void SaveChanges()
        {
            DBEntity.SaveChanges();
        }


        #region Disposing the Unit of work context ...
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DBEntity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}