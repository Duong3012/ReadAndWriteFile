using DemoProject.DAL.Interface;
using DemoProject.Models.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoProject.DAL.Implement
{
    public class GenericRepository<T> : IGenericReopository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public IEnumerable<T> GetList()
        {
            return _dbSet.AsEnumerable();
        }

        public T GetById(string Id)
        {
            return _dbSet.Find(Id);
        }


        public bool InsertObject(T obj)
        {
            try
            {
                var entity = _dbSet.Add(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool UpdateObject(T obj)
        {
            try
            {
                _dbSet.Update(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool DeleteObject(string Id)
        {
            try
            {
                var obj = _dbSet.Find(Id);
                _dbSet.Remove(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
