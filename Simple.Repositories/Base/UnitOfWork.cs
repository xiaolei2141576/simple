using Simple.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Repositories.Base
{
    public class UnitOfWork
    {
        private EfDbContext dbContext = null;

        public UnitOfWork()
        {
            dbContext = new EfDbContext();
        }
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IEfBaseRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (repositories.Keys.Contains(typeof(TEntity)) == true)
            {
                return repositories[typeof(TEntity)] as IEfBaseRepository<TEntity>;
            }
            IEfBaseRepository<TEntity> repo = new EfBaseRepository<TEntity>(dbContext);
            repositories.Add(typeof(TEntity), repo);
            return repo;
        }
        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
