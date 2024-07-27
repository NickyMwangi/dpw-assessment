using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class RepoService : IRepoService
    {
        protected readonly IdContext _dbContext;
        public RepoService(IdContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Query<T>(bool forUpdate = true) where T : BaseEntity
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (!forUpdate)
            {
                query.AsNoTracking();
            }
            foreach (var property in _dbContext.Model.FindEntityType(typeof(T)).GetNavigations())
            {
                query = query.Include(property.Name);
            }
            return query;
        }

        public T GetById<T>(string id) where T : BaseEntity
        {
            return Query<T>().SingleOrDefault(m => m.Id == id);
        }

        public async Task<T> GetByIdAsync<T>(string id) where T : BaseEntity
        {
            return await Query<T>().SingleOrDefaultAsync(m => m.Id == id);
        }

        public T GetById<T>(params object[] id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync<T>(params object[] id) where T : class
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetAll<T>() where T : BaseEntity
        {
            return Query<T>().AsEnumerable();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : BaseEntity
        {
            return await Query<T>().ToListAsync();
        }

        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, bool forUpdate = false) where T : BaseEntity
        {
            return Query<T>().Where(predicate).AsEnumerable();
        }

        public async Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate, bool forUpdate = false) where T : BaseEntity
        {
            return await Query<T>(forUpdate).Where(predicate).ToListAsync();
        }

        public T Insert<T>(T entity) where T : BaseEntity, new()
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public T Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }

        public T Modify<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            Save();
            return entity;
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            Save();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveAsync();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public T InsertUpdate<T>(T entity, bool isNew) where T : BaseEntity, new()
        {
            if (isNew)
                return Insert(entity);
            else
                return Update(entity);
        }

        //public IEnumerable<SelectListItem> SelectList<T>(Expression<Func<T, bool>> predicate,
        //   Expression<Func<T, SelectListItem>> selector) where T : BaseEntity
        //{
        //    IEnumerable<SelectListItem> ddl = _dbContext.Set<T>().Where(predicate).Select(selector).ToList();
        //    return ddl;
        //}

        //public IEnumerable<SelectListItem> SelectList<T>(Expression<Func<T, SelectListItem>> selector) where T : BaseEntity
        //{
        //    IEnumerable<SelectListItem> ddl = _dbContext.Set<T>().Select(selector).ToList();
        //    return ddl;
        //}
    }
}
