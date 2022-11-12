using DemoProject.DAL.Interface;
using DemoProject.DAL.Interface.UnitOfWork;
using DemoProject.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEmployeeRepository employeeRepository;
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (this.employeeRepository == null)
                {
                    this.employeeRepository = new EmployeeRepository(_db);
                }
                return employeeRepository;
            }
        }

        public ICourseRepository courseRepository;
        public ICourseRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new CourseRepository(_db);
                }
                return courseRepository;
            }
        }

        public IJobRepository jobRepository;
        public IJobRepository JobRepository
        {
            get
            {
                if (this.jobRepository == null)
                {
                    this.jobRepository = new JobRepository(_db);
                }
                return jobRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChange()
        {
            _db.SaveChanges();
        }
    }
}
