using DemoProject.DAL.Interface.UnitOfWork;
using DemoProject.Models;
using DemoProject.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Implement
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private ApplicationDbContext db;
        public CourseRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public bool InsertBulk(List<Course> Courses)
        {
            try
            {
                db.Courses.AddRange(Courses);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateBulk(List<Course> Courses)
        {
            try
            {
                db.Courses.UpdateRange(Courses);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
