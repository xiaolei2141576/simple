using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Simple.IRepositories.Base;
using Simple.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repositories.Base
{
    //public class EfBaseRepository<TEntity> : IEfBaseRepository<TEntity> where TEntity : class
    //{
    //    private EfDbContext _dbContext = null;
    //    DbSet<TEntity> _dbSet;

    //    public EfBaseRepository(EfDbContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //        _dbSet = _dbContext.Set<TEntity>();
    //    }
    //    #region 查询

    //    public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
    //    {
    //        return await Task.Run(() => _dbSet.Where(predicate).FirstOrDefault());
    //    }

    //    /// <summary>
    //    /// 单表查询
    //    /// </summary>
    //    /// <param name="predicate"></param>
    //    /// <returns></returns>
    //    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> predicate)
    //    {
    //        return await Task.Run(() => _dbSet.Where(predicate).ToList());
    //    }

    //    /// <summary>
    //    /// 多表关联查询
    //    /// </summary>
    //    /// <param name="predicate"></param>
    //    /// <param name="tableNames"></param>
    //    /// <returns></returns>
    //    public async Task<List<TEntity>> QueryJoin(Expression<Func<TEntity, bool>> predicate, string[] tableNames)
    //    {
    //        if (tableNames == null && tableNames.Any() == false)
    //        {
    //            throw new Exception("缺少连表名称");
    //        }
    //        IQueryable<TEntity> query = _dbSet.AsQueryable();
    //        foreach (var table in tableNames)
    //        {
    //            query = query.Include(table);
    //        }
    //        return await Task.Run(() => query.Where(predicate).ToList());
    //    }

    //    /// <summary>
    //    /// 升序查询还是降序查询
    //    /// </summary>
    //    /// <typeparam name="TKey"></typeparam>
    //    /// <param name="predicate"></param>
    //    /// <param name="keySelector"></param>
    //    /// <param name="isQueryOrderBy"></param>
    //    /// <returns></returns>
    //    public async Task<List<TEntity>> QueryOrderBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool isQueryOrderBy)
    //    {
    //        if (isQueryOrderBy)
    //        {
    //            return _dbSet.Where(predicate).OrderBy(keySelector).ToList();
    //        }
    //        return await Task.Run(() => _dbSet.Where(predicate).OrderByDescending(keySelector).ToList());
    //    }

    //    /// <summary>
    //    /// 升序分页查询还是降序分页
    //    /// </summary>
    //    /// <typeparam name="TKey"></typeparam>
    //    /// <param name="pageIndex">第几页</param>
    //    /// <param name="pagesize">一页多少条</param>
    //    /// <param name="rowCount">返回共多少条</param>
    //    /// <param name="predicate">查询条件</param>
    //    /// <param name="keySelector">排序字段</param>
    //    /// <param name="isQueryOrderBy">true为升序 false为降序</param>
    //    /// <returns></returns>
    //    public List<TEntity> QueryByPage<TKey>(int pageIndex, int pagesize, out int rowCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool isQueryOrderBy)
    //    {
    //        rowCount = _dbSet.Count(predicate);
    //        if (isQueryOrderBy)
    //        {
    //            return _dbSet.Where(predicate).OrderBy(keySelector).Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
    //        }
    //        else
    //        {
    //            return _dbSet.Where(predicate).OrderByDescending(keySelector).Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
    //        }
    //    }
    //    #endregion

    //    #region 编辑
    //    /// <summary>
    //    /// 通过传入的model加需要修改的字段来更改数据
    //    /// </summary>
    //    /// <param name="model"></param>
    //    /// <param name="propertys"></param>
    //    public async Task<bool> Update(TEntity model, string[] propertys)
    //    {
    //        if (model == null)
    //        {
    //            throw new Exception("实体不能为空");
    //        }
    //        if (!propertys.Any())
    //        {
    //            throw new Exception("要修改的属性至少要有一个");
    //        }
    //        var entry = _dbContext.Entry(model);
    //        foreach (var prop in propertys)
    //        {
    //            entry.Property(prop).IsModified = true;
    //        }
    //        return await Task.Run(() => _dbContext.Entry(model).State == EntityState.Modified);
    //        //关闭EF对于实体的合法性验证参数
    //        //_dbContext.Configuration. = false;
    //    }

    //    /// <summary>
    //    /// 直接查询之后再修改
    //    /// </summary>
    //    /// <param name="model"></param>
    //    public async Task<bool> Update(TEntity model)
    //    {
    //        return await Task.Run(() => _dbContext.Entry(model).State == EntityState.Modified);
    //    }
    //    #endregion

    //    #region 删除
    //    public async Task<bool> Delete(TEntity model, bool isadded)
    //    {
    //        if (!isadded)
    //        {
    //            _dbSet.Attach(model);
    //        }
    //        return await Task.Run(() => _dbContext.Remove(model).State == EntityState.Deleted);
    //    }
    //    #endregion

    //    #region 新增
    //    public async Task<bool> Insert(TEntity model)
    //    {
    //        return await Task.Run(() => _dbContext.Add(model).State == EntityState.Added);
    //    }
    //    #endregion

    //    #region 统一提交
    //    public async Task<int> SaveChanges()
    //    {
    //        return await _dbContext.SaveChangesAsync();
    //    }
    //    #endregion

    //    #region 调用存储过程返回一个指定的TResult
    //    //public List<TResult> RunProc<TResult>(string sql, params object[] pamrs)
    //    //{
    //    //    return _dbContext.Database.<TResult>(sql, pamrs).ToList();
    //    //}
    //    #endregion
    //}

    /// <summary>
    ///     仓储
    /// </summary>
    public class EfBaseRepository<TEntity> : IEfBaseRepository<TEntity> where TEntity : class
    {
        #region Fields
        //private readonly IUnitOfWork _efDbContext;
        private readonly DbContext _efDbContext;
        private readonly DbSet<TEntity> _dbSet;
        #endregion

        #region ctor
        /// <summary>
        /// 初始化一个<see cref="Repository{TEntity}"/>类型的新实例
        /// </summary>
        public EfBaseRepository(IUnitOfWork uow)
        {
            this._efDbContext = uow.DbContext;
            this._dbSet = _efDbContext.Set<TEntity>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取 当前实体类型的查询数据集，数据将使用不跟踪变化的方式来查询
        /// </summary>
        public IQueryable<TEntity> Entites { get { return _dbSet.AsNoTracking(); } }

        #endregion

        #region 方法

        ///// <summary>
        /////     判断指定表达式条件 的对象是否存在
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool CheckExists(Expression<Func<TEntity, bool>> predicate, object id)
        //{
        //    var entity = _dbSet.Where(predicate).Select(m => )
        //}
        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate, bool save = true)
        {
            var entities = _dbSet.Where(predicate).AsEnumerable();
            if (null != entities && entities.Count() > 0)
            {
                _dbSet.RemoveRange(entities);
            }
            return await _efDbContext.SaveChangesAsync(save);
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(IEnumerable<TEntity> entities, bool save = true)
        {
            if (null != entities && entities.Count() > 0)
            {
                _dbSet.RemoveRange(entities);
            }
            return await _efDbContext.SaveChangesAsync(save);
        }

        public async Task<int> Delete(object key, bool save = true)
        {
            var entity = _dbSet.Find(key);
            _dbSet.Remove(entity);
            return await _efDbContext.SaveChangesAsync(save);
        }

        public async Task<int> Delete(TEntity entity, bool save = true)
        {
            _dbSet.Remove(entity);
            return await _efDbContext.SaveChangesAsync(save);
        }

        public async Task<TEntity> GetByKey(object key)
        {
            return await _dbSet.FindAsync(key);
        }

        public async Task<IQueryable<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(()=> _dbSet.Where(predicate).AsQueryable());
        }

        public async Task<IQueryable<TEntity>> GetInclude<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return await Task.Run(() => _dbSet.Include(path));
        }

        //public async Task<IQueryable<TEntity>> GetIncludes(params string[] paths)
        //{
        //    IQueryable<TEntity> sources = null;
        //    foreach (var path in paths)
        //    {
        //        sources = _dbSet.Include(path);
        //    }
        //    return sources;
        //}

        public async Task<int> Insert(IEnumerable<TEntity> entities, bool save = true)
        {
            _dbSet.AddRange(entities);
            return await _efDbContext.SaveChangesAsync(save);
        }

        public async Task<int> Insert(TEntity entity, bool save = true)
        {
            _dbSet.Add(entity);
            return await _efDbContext.SaveChangesAsync(save);
        }

        //public IEnumerable<TEntity> SqlQuery(string sql, bool trackEnabled = true, params object[] parameters)
        //{
        //    return trackEnabled
        //         ? _dbSet.s(sql, parameters)
        //         : _dbSet.FromSql(sql, parameters).AsNoTracking();
        //}

        public async Task<int> Update(object key, bool save = true)
        {
            var entity = _dbSet.Find(key);
            return await Update(entity, save);
        }

        public async Task<int> Update(TEntity entity, bool save = true)
        {
            DbContext context = ((DbContext)_efDbContext);

            DbSet<TEntity> dbSet = context.Set<TEntity>();
            try
            {
                EntityEntry<TEntity> entry = context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message);
            }
            return await _efDbContext.SaveChangesAsync(save);
        }

        public async Task<int> Update(IEnumerable<TEntity> entites, bool save = true)
        {
            DbContext context = ((DbContext)_efDbContext);

            DbSet<TEntity> dbSet = context.Set<TEntity>();

            foreach (var entity in entites)
            {
                try
                {
                    EntityEntry<TEntity> entry = context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return await _efDbContext.SaveChangesAsync(save);
        }

        public IQueryable<TEntity> QueryPage<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, out int totalRow, bool isQueryOrderBy = true, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            IQueryable<TEntity> source = _dbSet.Where(predicate);
            if (isQueryOrderBy)
            {
                source = source.OrderBy(orderBy);
            }
            else
            {
                source = source.OrderByDescending(orderBy);
            }
            totalCount = source.Count();
            source = source != null
                ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : Enumerable.Empty<TEntity>().AsQueryable();
            totalRow = totalCount;
            return source;
        }

        public async Task<List<TEntity>> QueryJoin(Expression<Func<TEntity, bool>> predicate, string[] tableNames)
        {
            if (tableNames == null && tableNames.Any() == false)
            {
                throw new Exception("缺少连表名称");
            }
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            foreach (var table in tableNames)
            {
                query = query.Include(table);
            }
            return await Task.Run(() => query.Where(predicate).ToList());
        }

        #endregion
    }
}
