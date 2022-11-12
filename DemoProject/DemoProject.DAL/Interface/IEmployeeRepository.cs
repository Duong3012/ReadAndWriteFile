using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Interface
{
    public interface IEmployeeRepository : IGenericReopository<Employee>
    {
        bool InsertBulk(List<Employee> employees);

        bool UpdateBulk(List<Employee> employees);

    }
}
