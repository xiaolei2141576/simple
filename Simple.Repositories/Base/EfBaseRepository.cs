using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Simple.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repositories.Base
{
    public class EfBaseRepository<TEntity> : IEfBaseRepository<TEntity> where TEntity : class
    {
        private EfDbContext _dbContext = null;
        DbSet<TEntity> _dbSet;

        public EfBaseRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        #region 查询

        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => _dbSet.Where(predicate).FirstOrDefault());
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => _dbSet.Where(predicate).ToList());
        }

        /// <summary>
        /// 多表关联查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 升序查询还是降序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="keySelector"></param>
        /// <param name="isQueryOrderBy"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryOrderBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool isQueryOrderBy)
        {
            if (isQueryOrderBy)
            {
                return _dbSet.Where(predicate).OrderBy(keySelector).ToList();
            }
            return await Task.Run(() => _dbSet.Where(predicate).OrderByDescending(keySelector).ToList());
        }

        /// <summary>
        /// 升序分页查询还是降序分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pagesize">一页多少条</param>
        /// <param name="rowCount">返回共多少条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="isQueryOrderBy">true为升序 false为降序</param>
        /// <returns></returns>
        public List<TEntity> QueryByPage<TKey>(int pageIndex, int pagesize, out int rowCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool isQueryOrderBy)
        {
            rowCount = _dbSet.Count(predicate);
            if (isQueryOrderBy)
            {
                return _dbSet.Where(predicate).OrderBy(keySelector).Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                return _dbSet.Where(predicate).OrderByDescending(keySelector).Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 通过传入的model加需要修改的字段来更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertys"></param>
        public async Task<bool> Update(TEntity model, string[] propertys)
        {
            if (model == null)
            {
                throw new Exception("实体不能为空");
            }
            if (!propertys.Any())
            {
                throw new Exception("要修改的属性至少要有一个");
            }
            var entry = _dbContext.Entry(model);
            foreach (var prop in propertys)
            {
                entry.Property(prop).IsModified = true;
            }
            return await Task.Run(() => _dbContext.Entry(model).State == EntityState.Modified);
            //关闭EF对于实体的合法性验证参数
            //_dbContext.Configuration. = false;
        }

        /// <summary>
        /// 直接查询之后再修改
        /// </summary>
        /// <param name="model"></param>
        public async Task<bool> Update(TEntity model)
        {
            return await Task.Run(() => _dbContext.Entry(model).State == EntityState.Modified);
        }
        #endregion

        #region 删除
        public async Task<bool> Delete(TEntity model, bool isadded)
        {
            if (!isadded)
            {
                _dbSet.Attach(model);
            }
            return await Task.Run(() => _dbContext.Remove(model).State == EntityState.Deleted);
        }
        #endregion

        #region 新增
        public async Task<bool> Insert(TEntity model)
        {
            return await Task.Run(() => _dbContext.Add(model).State == EntityState.Added);
        }
        #endregion

        #region 统一提交
        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region 调用存储过程返回一个指定的TResult
        //public List<TResult> RunProc<TResult>(string sql, params object[] pamrs)
        //{
        //    return _dbContext.Database.<TResult>(sql, pamrs).ToList();
        //}
        #endregion
    }
}
