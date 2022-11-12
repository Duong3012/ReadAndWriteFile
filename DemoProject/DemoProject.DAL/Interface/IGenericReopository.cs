using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Interface
{
    public interface IGenericReopository<T> where T : class
    {
        IEnumerable<T> GetList();
        T GetById(string Id);

        bool InsertObject(T obj);

        bool UpdateObject(T obj);

        bool DeleteObject(string Id);
    }
}
