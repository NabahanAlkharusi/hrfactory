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
