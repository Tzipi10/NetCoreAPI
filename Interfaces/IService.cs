using System.Collections.Generic;

namespace MyApi.Interfaces
{
    public interface IService<T>
    {
        List<T> Get();
        T Get(int id);
        int Insert(T newItem);
        bool Update(int id, T newItem);
        bool Delete(int id);

    }
}
