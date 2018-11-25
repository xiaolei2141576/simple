using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Simple.IServices.Base;
using System.Threading.Tasks;
using Simple.IRepositories.Base;

namespace Simple.Services.Base
{
    public class EfBaseService<TEntity> : IEfBaseService<TEntity> where TEntity : class, new()
    {
        public IEfBaseRepository<TEntity> baseDal;

        ///// <summary>
        ///// 单表查询 单条数据
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return baseDal.Single(predicate);
        //}
        /// <summary>
        /// 单表查询 单条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            return await baseDal.Single(predicate);
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
       public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return await baseDal.Query(predicate);
        }

        /// <summary>
        /// 多表关联查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryJoin(Expression<Func<TEntity, bool>> predicate, string[] tableNames)
        {
            return await baseDal.QueryJoin(predicate, tableNames);
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
            return await baseDal.QueryOrderBy(predicate, keySelector, isQueryOrderBy);
        }

        /// <summary>
        /// 升序分页查询还是降序分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">一页多少条</param>
        /// <param name="rowCount">返回共多少条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="isQueryOrderBy">true为升序 false为降序</param>
        /// <returns></returns>
        public List<TEntity> QueryByPage<TKey>(int pageIndex, int pageSize, out int rowCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool isQueryOrderBy)
        {
            return baseDal.QueryByPage(pageIndex, pageSize, out rowCount, predicate, keySelector, isQueryOrderBy);
        }

        /// <summary>
        /// 通过传入的model加需要修改的字段来更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertys"></param>
        public async Task<bool> Update(TEntity model, string[] propertys)
        {
            return await baseDal.Update(model, propertys);
        }

        /// <summary>
        /// 直接查询之后再修改
        /// </summary>
        /// <param name="model"></param>
        public async Task<bool> Update(TEntity model)
        {
            return await baseDal.Update(model);
        }

        public async Task<bool> Delete(TEntity model, bool isadded)
        {
            return await baseDal.Delete(model, isadded);
        }

        public async Task<bool> Insert(TEntity model)
        {
            return await baseDal.Insert(model);
        }

        public async Task<int> SaveChanges()
        {
            return await baseDal.SaveChanges();
        }
    }
}
