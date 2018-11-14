using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepositories.Base
{
    /// <summary>
    ///     工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IEfBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;

        void BeginTransaction();

        int Commit();
        Task<int> CommitAsync();
    }
}
