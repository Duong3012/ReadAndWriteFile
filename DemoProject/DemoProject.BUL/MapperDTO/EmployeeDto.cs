using CsvHelper.Configuration;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.BUL.MapperDTO
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }

    public sealed class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Map(m => m.EmpNo).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.Address).Name("Address");
            Map(m => m.Email).Name("Email");
            Map(m => m.Description).Name("Description");
        }
    }
}
