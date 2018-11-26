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

        public async Task<int> Insert(TEntity model, bool save = true)
        {
            return await baseDal.Insert(model, save);
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public async Task<int> Insert(IEnumerable<TEntity> entities, bool save = true)
        {
            return await baseDal.Insert(entities, save);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(TEntity entity, bool save = true)
        {
            return await baseDal.Delete(entity, save);
        }
        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(object key, bool save = true)
        {
            return await baseDal.Delete(key, save);
        }
        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate, bool save = true)
        {
            return await baseDal.Delete(predicate, save);
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Delete(IEnumerable<TEntity> entities, bool save = true)
        {
            return await baseDal.Delete(entities, save);
        }
        /// <summary>
        ///     更新指定主键的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<int> Update(object key, bool save = true)
        {
            return await baseDal.Update(key, save);
        }

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> Update(TEntity entity, bool save = true)
        {
            return await baseDal.Update(entity, save);
        }
        /// <summary>
        ///     批量更新数据
        /// </summary>
        /// <param name="entites">对象集合</param>
        /// <returns></returns>
        public async Task<int> Update(IEnumerable<TEntity> entites, bool save = true)
        {
            return await baseDal.Update(entites, save);
        }
        ///// <summary>
        ///// 检查实体是否存在
        ///// </summary>
        ///// <param name="predicate">查询条件谓语表达式</param>
        ///// <param name="id">编辑的实体标识</param>
        ///// <returns>是否存在</returns>
        //bool CheckExists(Expression<Func<TEntity, bool>> predicate, object id);

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public async Task<TEntity> GetByKey(object key)
        {
            return await baseDal.GetByKey(key);
        }

        /// <summary>
        /// 查询指定条件的实体
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>符合条件的实体集合</returns>
        public async Task<IQueryable<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            return await baseDal.GetByPredicate(predicate);
        }

        /// <summary>
        /// 获取贪婪加载导航属性的查询数据集
        /// </summary>
        /// <param name="path">属性表达式，表示要贪婪加载的导航属性</param>
        /// <returns>查询数据集</returns>
        public async Task<IQueryable<TEntity>> GetInclude<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return await baseDal.GetInclude(path);
        }

        ///// <summary>
        ///// 获取贪婪加载多个导航属性的查询数据集
        ///// </summary>
        ///// <param name="paths">要贪婪加载的导航属性名称数组</param>
        ///// <returns>查询数据集</returns>
        //public async Task<IQueryable<TEntity>> GetIncludes(params string[] paths)
        //{

        //}

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回此集中的实体。 
        /// 默认情况下，上下文会跟踪返回的实体；可通过对返回的 DbRawSqlQuery 调用 AsNoTracking 来更改此设置。 请注意返回实体的类型始终是此集的类型，而不会是派生的类型。 如果查询的一个或多个表可能包含其他实体类型的数据，则必须编写适当的 SQL 查询以确保只返回适当类型的实体。 与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。 您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。 您提供的任何参数值都将自动转换为 DbParameter。 context.Set(typeof(Blog)).SqlQuery("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。 这允许您在 SQL 查询字符串中使用命名参数。 context.Set(typeof(Blog)).SqlQuery("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <param name="trackEnabled">是否跟踪返回实体</param>
        /// <param name="sql">SQL 查询字符串。</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。 如果使用输出参数，则它们的值在完全读取结果之前不可用。 这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        /// <returns></returns>
        //IEnumerable<TEntity> SqlQuery(string sql, bool trackEnabled = true, params object[] parameters);

        /// <summary>
        ///     分页数据查询
        /// </summary>
        /// <param name="pageCondition">分页和排序条件</param>
        /// <param name="predicate">数据过滤条件 表达式</param>
        /// <returns>分页后的数据集合</returns>
        public IQueryable<TEntity> QueryPage<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, out int totalRow, bool isQueryOrderBy = true, int pageIndex = 1, int pageSize = 20)
        {
            return baseDal.QueryPage(predicate, orderBy, out totalRow, isQueryOrderBy, pageIndex, pageSize);
        }

        public async Task<List<TEntity>> QueryJoin(Expression<Func<TEntity, bool>> predicate, string[] tableNames)
        {
            return baseDal.qu
        }
    }
}
