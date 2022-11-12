using DemoProject.DAL.Interface;
using DemoProject.Models;
using DemoProject.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Implement
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        private ApplicationDbContext db;
        public JobRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db; 
        }

        public bool InsertBulk(List<Job> jobs)
        {
            try
            {
                db.Jobs.AddRange(jobs);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateBulk(List<Job> jobs)
        {
            try
            {
                db.Jobs.UpdateRange(jobs);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
