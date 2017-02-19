namespace Data.Common
{
    using Data.Common.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DbRepository<T> : DbRepository<T, int>, IDbRepository<T>
       where T : GenericModel<int>
    {
        public DbRepository(DbContext context)
            : base(context)
        {
        }
    }

    public class DbRepository<T, K> : IDbRepository<T, K>
        where T : GenericModel<int>
        where K : struct
    {
        private IDbSet<T> DbSet { get; set; }
        private DbContext Context { get; set; }

        public DbRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.DbSet;
        }
        public T GetById(K id)
        {
            return this.DbSet.Find(id);
        }
        public void Add(T entity)
        {
            this.DbSet.Add(entity);
        }
        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }
        public void Delete(T entity)
        {
            this.DbSet.Remove(entity);
        }
        public int Save()
        {
            return this.Context.SaveChanges();
        }

        public void Attach(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
        }
        public void AttachModel<M>(M entity) where M : class
        {
            var entry = this.Context.Entry<M>(entity);
            if (entry.State == EntityState.Detached)
            {
                this.Context.Set<M>().Attach(entity);
            }
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = state;
        }
    }
}
