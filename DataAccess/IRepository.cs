using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<T>
    {
        Task Add(T t);
        Task<T> RetrieveById(Guid id);
        IQueryable<T> RetrieveAll();
        Task Delete(T t);
        Task DeleteById(Guid id);
        Task Update(T t);
    }
}
