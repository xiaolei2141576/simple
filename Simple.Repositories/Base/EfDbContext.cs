using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simple.Repositories.Base
{
    public class EfDbContext : DbContext
    {
        private DbContextOption _option;

        public IConfiguration Configuration { get; }
        public EfDbContext()
        {
            _option = new DbContextOption
            {
                ConnectionString = Configuration.GetSection("AppSettings:SqlServerConnection").Value,
                ModelAssemblyName = Configuration.GetSection("ModelAssemblyName").Value,
                //ModelAssemblyName = "Simple.Models",
                DbType = DbType.MSSQLSERVER
            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void AddEntityTypes(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.Load(_option.ModelAssemblyName);
            var types = assembly?.GetTypes();
            var list = types?.Where(t =>
                t.IsClass && !t.IsGenericType && !t.IsAbstract &&
                t.GetInterfaces().Any(m => m.IsGenericType)).ToList();
            if (list != null && list.Any())
            {
                list.ForEach(t =>
                {
                    if (modelBuilder.Model.FindEntityType(t) == null)
                        modelBuilder.Model.AddEntityType(t);
                });
            }
        }

        public class DbContextOption
        {
            /// <summary>
            /// 数据库连接字符串
            /// </summary>
            public string ConnectionString { get; set; }
            /// <summary>
            /// 实体程序集名称
            /// </summary>
            public string ModelAssemblyName { get; set; }
            /// <summary>
            /// 数据库类型
            /// </summary>
            public DbType DbType { get; set; } = DbType.MSSQLSERVER;
        }

        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        public enum DbType
        {
            /// <summary>
            /// MS SQL Server
            /// </summary>
            MSSQLSERVER = 0,
            /// <summary>
            /// Oracle
            /// </summary>
            ORACLE,
            /// <summary>
            /// MySQL
            /// </summary>
            MYSQL,
            /// <summary>
            /// Sqlite
            /// </summary>
            SQLITE,
            /// <summary>
            /// in-memory database
            /// </summary>
            MEMORY,
            /// <summary>
            /// PostgreSQL
            /// </summary>
            NPGSQL
        }
    }
}
