using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Interface.UnitOfWork
{
    public interface ICourseRepository : IGenericReopository<Course>
    {
        bool InsertBulk(List<Course> Courses);

        bool UpdateBulk(List<Course> Courses);
    }
}
