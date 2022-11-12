using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DemoProject.BUL.Interface
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetList();
        Employee GetById(string Id);
        bool Insert(Employee obj);
        bool Update(Employee obj);
        bool Delete(string Id);
        bool ReadFiletoDatatable(string filePath);

        public DataTable ToDataTable<T>(List<T> items);

        byte[] ExportExcel(List<Employee> employees);
    }
}
