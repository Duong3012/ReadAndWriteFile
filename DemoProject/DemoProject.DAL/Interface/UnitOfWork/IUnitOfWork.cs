using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Interface.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IJobRepository JobRepository { get; }
        public void SaveChange();
    }
}
