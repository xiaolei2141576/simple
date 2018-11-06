using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Generic.Repository.Domain.Interfaces;
using Simple.Repositories.Configurations;
using Simple.Repositories.Entities;

// add using root namespace where generic.repository nuget package was installed if it different.

namespace Simple.Repositories
{
    public class DataContext : DbContext, IDataContext
    {
        private readonly string _schemaName;

        public DataContext(DbContextOptions<DataContext> options, string schemaName = null)
            : base(options)
        {
            _schemaName = schemaName;

            #region Repository           
            
            // Instance repositories properties - Sample:
            // SampleRepository = new SampleRepository(this);

            #endregion
        }

        public string SchemaName { get { return _schemaName; } }

        #region DbSet        

        // Insert DbSet properties - Sample:
        // public DbSet<Sample> Sample { get; set; }

        #endregion

        #region Repository Properties  

        // Insert IRepository properties - Sample:
        // public ISampleRepository SampleRepository { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityCongurationMapper<>));

            // Add all fluent API configurations
            foreach (var entityTypeConfig in entityTypeConfigurations)
            {
                dynamic instance = Activator.CreateInstance(entityTypeConfig);
                modelBuilder.ApplyConfiguration(instance);
            }

            if(!string.IsNullOrEmpty(SchemaName))
                modelBuilder.HasDefaultSchema(SchemaName);                 
        }

        public Task CommitAsync()
        {
            return this.SaveChangesAsync();
        }        
    }
}