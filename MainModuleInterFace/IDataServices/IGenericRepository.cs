using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainModuleInterFace.IDataServices
{
    public interface IGenericRepository<T> where T : class
    {
        int GetTotalRows(Expression<System.Func<T, bool>> predicate);

        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(List<T> entities);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
        Task<int> SaveAsync();
        Task<int> DeleteRangeExistingList(List<T> deleteList);
        void Reload(T entity);
    }
}
