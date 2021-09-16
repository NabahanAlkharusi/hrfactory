using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess
{
    public interface IRepository<T>
    {
        T FindById(object id);
        void Add(T obj);
        void Update(T obj);
        void Remove(object id);
        void SaveChanges();
    }
}
