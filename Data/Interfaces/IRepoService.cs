using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IRepoService
{
    IQueryable<T> Query<T>(bool forUpdate = true) where T : BaseEntity;
    T GetById<T>(string id) where T : BaseEntity;
    Task<T> GetByIdAsync<T>(string id) where T : BaseEntity;
    T GetById<T>(params object[] id) where T : class;
    Task<T> GetByIdAsync<T>(params object[] id) where T : class;
    IEnumerable<T> GetAll<T>() where T : BaseEntity;
    Task<List<T>> GetAllAsync<T>() where T : BaseEntity;
    IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, bool forUpdate = true) where T : BaseEntity;
    Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate, bool forUpdate = true) where T : BaseEntity;
    T Insert<T>(T entity) where T : BaseEntity, new();
    T Update<T>(T entity) where T : BaseEntity;
    T Modify<T>(T entity) where T : BaseEntity;
    T InsertUpdate<T>(T entity, bool isNew) where T : BaseEntity, new();
    void Delete<T>(T entity) where T : BaseEntity;
    Task DeleteAsync<T>(T entity) where T : BaseEntity;
    void Save();
    Task SaveAsync();
    //IEnumerable<SelectListItem> SelectList<T>(
    //   Expression<Func<T, SelectListItem>> selector) where T : BaseEntity;
    //IEnumerable<SelectListItem> SelectList<T>(Expression<Func<T, bool>> predicate,
    //   Expression<Func<T, SelectListItem>> selector) where T : BaseEntity;
}
