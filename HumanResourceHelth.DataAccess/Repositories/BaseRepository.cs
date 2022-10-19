using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly HumanResourceContext _context;
        private readonly DbSet<T> _table;
        public BaseRepository()
        {
            _context = new HumanResourceContext();
            _table = _context.Set<T>();

        }
        public BaseRepository(HumanResourceContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }
        public T FindById(object id)
        {
            return _table.Find(id);
        }

        public virtual void Add(T obj)
        {
            _table.Add(obj);
        }

        public int Count()
        {
            return _table.Count();
        }

        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void BulkUpdate(List<T> obj)
        {
            foreach (var item in obj)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
        }
        public void Remove(object id)
        {
            T existing = _table.Find(id);
            if (existing != null) _table.Remove(existing);
        }

        public void RemoveObject(T obj)
        {
            _table.Remove(obj);
        }

        public void RemoveBulk(List<T> objList)
        {
            _table.RemoveRange(objList);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> predicate, int accountId)
        {
            return _context.Set<T>().Where(predicate);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAll(int accountId)
        {
            return _context.Set<T>().ToList();
        }
        public List<T> GetAll(string include)
        {
            return _context.Set<T>().Include(include).ToList();
        }
        public void DeleteAll()
        {
            _context.Set<T>().RemoveRange(GetAll());
        }

        public void AddBulk(List<T> obj)
        {
            _context.Set<T>().AddRange(obj);
        }
        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }
        public void DeleteBulk(List<T> obj)
        {
            _context.Set<T>().RemoveRange(obj);
        }
    }
}
