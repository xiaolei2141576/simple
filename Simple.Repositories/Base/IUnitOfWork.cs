using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repositories.Base
{
    ///// <summary>
    /////     工作单元接口
    ///// </summary>
    //public interface IUnitOfWork : IDisposable
    //{
    //    IEfBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;

    //    void BeginTransaction();

    //    int Commit();
    //    Task<int> CommitAsync();
    //}

    /// <summary>
    ///     
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        #region 方法

        EfDbContext DbContext { get; }

        /// <summary>
        ///     命令提交
        /// </summary>
        /// <returns>提交操作结果</returns>
        int SaveChanges(bool isSave);

        Task<int> SaveChangesAsync(bool isSave);
        #endregion


    }
}
