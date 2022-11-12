using DemoProject.DAL.Interface;
using DemoProject.Models;
using DemoProject.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Implement
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private ApplicationDbContext db;
        public EmployeeRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public bool InsertBulk(List<Employee> employees)
        {
            try
            {
                db.Employees.AddRange(employees);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool UpdateBulk(List<Employee> employees)
        {
            try
            {
                db.Employees.UpdateRange(employees);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
