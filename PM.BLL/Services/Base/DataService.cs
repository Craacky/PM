using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PM.DAL.EFCore;
using PM.DAL.Entities.Base;
using System.Linq.Expressions;

namespace PM.BLL.Services.Base
{
    public class DataService<T> where T : Entity
    {
        public string ConnectionString { get; set; }
        public DbSet<T> DbSet { get; set; }


        public DataService(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public T Create(T entity)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                EntityEntry<T> createdResult = dbContext.Set<T>().Add(entity);
                dbContext.SaveChanges();

                return createdResult.Entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> CreateAsync(T entity)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                EntityEntry<T> createdResult = await dbContext.Set<T>().AddAsync(entity);
                await dbContext.SaveChangesAsync();

                return createdResult.Entity;
            }
            else
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = dbContext.Set<T>().FirstOrDefault(e => e.Id == id);
                dbContext.Set<T>().Remove(entity);
                dbContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                dbContext.Set<T>().Remove(entity);
                await dbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public T Get(int id)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = dbContext.Set<T>().FirstOrDefault(e => e.Id == id);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> GetAsync(int id)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = dbContext.Set<T>().FirstOrDefault(predicate);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
                return entity;
            }
            else
            {
                return null;
            }
        }

        public T GetWithInclude<TProperty>(int id, Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = dbContext.Set<T>().Include(includePredicate).FirstOrDefault(e => e.Id == id);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> GetWithIncludeAsync<TProperty>(int id, Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().Include(includePredicate).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public T GetWithInclude<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = dbContext.Set<T>().Include(includePredicate).FirstOrDefault(predicate);
                return entity;
            }
            else
            {
                return null;
            }
        }


        public async Task<T> GetWithIncludeAsync<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().Include(includePredicate).FirstOrDefaultAsync(predicate);
                return entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> GetWithIncludeAsync<TProperty, TProperty2>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TProperty>>> includePredicate, Expression<Func<TProperty, TProperty2>> thenIncludePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                T entity = await dbContext.Set<T>().Include(includePredicate).ThenInclude(thenIncludePredicate).FirstOrDefaultAsync(predicate);
                return entity;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selectPredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<TResult> entities = dbContext.Set<T>().Select(selectPredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            using DBContext dbContext = new(ConnectionString);
            IEnumerable<T> entities = dbContext.Set<T>().Where(predicate).ToList();

            return entities;
        }
        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectPredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<TResult> entities = dbContext.Set<T>().Where(predicate).Select(selectPredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty>(Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty, TProperty2>(Expression<Func<T, IEnumerable<TProperty>>> includePredicate, Expression<Func<TProperty, TProperty2>> thenIncludePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ThenInclude(thenIncludePredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty, TProperty2>(Expression<Func<T, TProperty>> includePredicate, Expression<Func<TProperty, TProperty2>> thenIncludePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ThenInclude(thenIncludePredicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TProperty>>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).Where(predicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> includePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).Where(predicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty, TProperty2>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TProperty>>> includePredicate, Expression<Func<TProperty, TProperty2>> thenIncludePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ThenInclude(thenIncludePredicate).Where(predicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<T> GetAllWithInclude<TProperty, TProperty2>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> includePredicate, Expression<Func<TProperty, TProperty2>> thenIncludePredicate)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                IEnumerable<T> entities = dbContext.Set<T>().Include(includePredicate).ThenInclude(thenIncludePredicate).Where(predicate).ToList();
                return entities;
            }
            else
            {
                return null;
            }
        }

        public T Update(int id, T entity)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                entity.Id = id;

                dbContext.Set<T>().Update(entity);
                dbContext.SaveChanges();

                return entity;
            }
            else
            {
                return null;
            }
        }
        public async Task<T> UpdateAsync(int id, T entity)
        {
            using DBContext dbContext = new(ConnectionString);
            if (dbContext.IsConnected)
            {
                entity.Id = id;

                dbContext.Set<T>().Update(entity);
                await dbContext.SaveChangesAsync();

                return entity;
            }
            else
            {
                return null;
            }
        }
    }
}
