using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainModuleInterFace.IDataServices;
using System.Linq.Expressions;

namespace MainModuleDataServices.Repository
{
    public abstract class GenericRepository<C, T> : IGenericRepository<T>
        where T : class
        where C : DbContext, new()
    {
        private C _entities = new C();
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }
        }


        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>().AsNoTracking();
            return query;
        }

        public virtual int GetTotalRows(Expression<System.Func<T, bool>> predicate)
        {
            int query = _entities.Set<T>().Where(predicate).Count();
            return query;
        }

        public IQueryable<T> FindBy(Expression<System.Func<T, bool>> predicate)
        {
            //.Configuration.AutoDetectChangesEnabled = true;
            var query = _entities.Set<T>().Where(predicate);
            // _entities.Configuration.AutoDetectChangesEnabled = false;
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void AddRange(List<T> entities)
        {
            _entities.Set<T>().AddRange(entities);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual async Task<int> DeleteRangeExistingList(List<T> deleteList)
        {
            _entities.Set<T>().RemoveRange(deleteList);

            return await _entities.SaveChangesAsync();
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();

        }

        public virtual async Task<int> SaveAsync()
        {
            return await _entities.SaveChangesAsync();
        }
        public virtual void DeleteRange(Expression<System.Func<T, bool>> predicate)
        {
            var res = _entities.Set<T>().Where(predicate).ToList();
            if (res.Count() > 0)
            {
                _entities.RemoveRange(res);
            }


        }
        public virtual void Reload(T entity)
        {
            try
            {
                _entities.Entry(entity).GetDatabaseValues();
            }
            catch { }
        }
      
    }
}
