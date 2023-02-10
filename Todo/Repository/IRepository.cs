using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Todo.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
        void Dispose();
    }
}