using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Simple.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repositories.Base
{
    public class UnitOfWork
    {
        private EfDbContext dbContext = null;
        /// <summary>
        ///     服务提供器，主要用于查找 框架配置对象，以及DbContextOptionBuilder对象
        /// </summary>
        private readonly IServiceProvider _provider;
        private IDbContextTransaction _dbTransaction { get; set; }
        public UnitOfWork(IServiceProvider provider)
        {
            dbContext = new EfDbContext();
            _provider = provider;
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

        public void BeginTransaction()
        {
            //DbContext.Database.UseTransaction(_dbTransaction);//如果多上下文，我们可是在其他上下文直接使用 UserTransaction使用已存在的事务
            _dbTransaction = dbContext.Database.BeginTransaction();
        }
        public int Commit()
        {
            int result = 0;
            try
            {
                result = dbContext.SaveChanges();
                if (_dbTransaction != null)
                    _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                result = -1;
                CleanChanges(dbContext);
                _dbTransaction.Rollback();
                throw new Exception($"Commit 异常：{ex.InnerException}/r{ ex.Message}");
            }
            return result;
        }

        public async Task<int> CommitAsync()
        {
            int result = 0;
            try
            {
                result = await dbContext.SaveChangesAsync();
                if (_dbTransaction != null)
                    _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                result = -1;
                CleanChanges(dbContext);
                _dbTransaction.Rollback();
                throw new Exception($"Commit 异常：{ex.InnerException}/r{ ex.Message}");
            }
            return await Task.FromResult(result);
        }


        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        /// <summary>
        ///     操作失败，还原跟踪状态
        /// </summary>
        /// <param name="context"></param>
        private static void CleanChanges(EfDbContext context)
        {
            var entries = context.ChangeTracker.Entries().ToArray();
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].State = EntityState.Detached;
            }
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
            _dbTransaction.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
